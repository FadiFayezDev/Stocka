using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Enums;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.Purchase
{
    public class ReceivePurchaseBatchCommand : IRequest<PurchaseWithItemsDto>
    {
        public Guid PurchaseId { get; set; }
        public List<ReceivePurchaseLineDto> Lines { get; set; } = new();
    }

    public class ReceivePurchaseLineDto
    {
        public Guid PurchaseItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public List<WarehouseAllocationDto> Allocations { get; set; } = new();
    }

    public class WarehouseAllocationDto
    {
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
    }

    public class ReceivePurchaseBatchCommandHandler : IRequestHandler<ReceivePurchaseBatchCommand, PurchaseWithItemsDto>
    {
        private readonly IPurchaseCommandRepository _purchaseRepo;
        private readonly IBatchCommandRepository _batchRepo;
        private readonly IWarehouseCommandRepository _warehouseRepo;
        private readonly IStockMovementCommandRepository _stockMovementRepo;
        private readonly ICurrentUserContext _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReceivePurchaseBatchCommandHandler(
            IPurchaseCommandRepository purchaseRepo,
            IBatchCommandRepository batchRepo,
            IWarehouseCommandRepository warehouseRepo,
            IStockMovementCommandRepository stockMovementRepo,
            ICurrentUserContext currentUser,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _purchaseRepo = purchaseRepo;
            _batchRepo = batchRepo;
            _warehouseRepo = warehouseRepo;
            _stockMovementRepo = stockMovementRepo;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PurchaseWithItemsDto> Handle(ReceivePurchaseBatchCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var purchase = await _purchaseRepo.GetByIdAsync(request.PurchaseId);
            if (purchase == null || purchase.BrandId.Value != brandId)
                throw new NotFoundException("أمر الشراء المطلوب غير موجود.");

            if (purchase.Status == Domain.Enums.PurchaseStatus.Cancelled)
                throw new BadRequestException("لا يمكن استلام أمر شراء ملغي.");

            if (!request.Lines.Any())
                throw new BadRequestException("يجب إدخال كمية مستلمة واحدة على الأقل.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var warehouses = new Dictionary<Guid, Warehouse>();

                foreach (var line in request.Lines)
                {
                    var item = purchase.PurchaseItems.FirstOrDefault(i => i.Id.Value == line.PurchaseItemId);
                    if (item == null)
                        throw new BadRequestException("أحد الأصناف غير موجود في أمر الشراء.");

                    if (line.Quantity <= 0)
                        throw new BadRequestException("كمية الدفعة المستلمة يجب أن تكون أكبر من صفر.");

                    if (line.Quantity > item.RemainingToReceive)
                        throw new BadRequestException($"الكمية المستلمة تتجاوز المتبقي المطلوب للصنف (المتبقي: {item.RemainingToReceive}).");

                    if (line.UnitCost <= 0)
                        throw new BadRequestException("سعر تكلفة الدفعة يجب أن يكون أكبر من صفر.");

                    if (line.Allocations.Sum(a => a.Quantity) != line.Quantity)
                        throw new BadRequestException("مجموع الكميات الموزعة على المخازن يجب أن يساوي الكمية المستلمة.");

                    if (line.Allocations.Any(a => a.Quantity < 0))
                        throw new BadRequestException("كمية التوزيع على المخزن يجب أن تكون أكبر من أو تساوي صفر.");

                    var batch = new Batch(
                        item.ProductId,
                        item.Id,
                        new BrandId(brandId),
                        line.Quantity,
                        line.UnitCost);

                    await _batchRepo.CreateAsync(batch);

                    foreach (var allocation in line.Allocations)
                    {
                        if (!warehouses.TryGetValue(allocation.WarehouseId, out var warehouse))
                        {
                            warehouse = await _warehouseRepo.GetByIdAsync(allocation.WarehouseId);
                            if (warehouse == null || warehouse.BrandId.Value != brandId)
                                throw new BadRequestException("أحد المخازن المحددة غير موجود.");
                            warehouses[allocation.WarehouseId] = warehouse;
                        }

                        warehouse.AddBatch(batch.Id, new BrandId(brandId), allocation.Quantity);
                        await _warehouseRepo.UpdateAsync(warehouse);

                        var movement = new StockMovement(
                            item.ProductId,
                            batch.Id,
                            warehouse.Id,
                            new BrandId(brandId),
                            allocation.Quantity,
                            StockMovementType.PurchaseIn,
                            StockReferenceType.Purchase,
                            purchase.Id.Value);

                        await _stockMovementRepo.CreateAsync(movement);
                    }

                    item.AddReceived(line.Quantity);
                }

                if (!purchase.HasPendingItems)
                    purchase.MarkCompleted();

                await _purchaseRepo.UpdateAsync(purchase);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<PurchaseWithItemsDto>(purchase);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}