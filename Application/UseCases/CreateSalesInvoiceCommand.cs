//using Application.Common.Exceptions;
//using Application.Common.Interfaces;
//using Application.Dtos.Orders;
//using Domain.Entities.Products;
//using Domain.Enums;
//using Domain.Repositories.Commands;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Application.UseCases
//{
//    public class CreateSalesInvoiceCommand : IRequest<bool>
//    {
//        public Guid BrandId { get; init; }
//        public Guid CustomerId { get; init; } // لو في عملاء
//        public List<OrderItemDto> Items { get; init; } = new();
//    }

//    public class CreateSalesInvoiceCommandHandler : IRequestHandler<CreateSalesInvoiceCommand, bool>
//    {
//        private readonly IWarehouseBatchCommandRepository _batchRepository;
//        private readonly IStockMovementCommandRepository _movementRepository;
//        private readonly IUnitOfWork _unitOfWork;

//        public CreateSalesInvoiceCommandHandler(IWarehouseBatchCommandRepository batchRepository, IStockMovementCommandRepository movementRepository, IUnitOfWork unitOfWork)
//        {
//            _batchRepository = batchRepository;
//            _movementRepository = movementRepository;
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<bool> Handle(CreateSalesInvoiceCommand request, CancellationToken cancellationToken)
//        {
//            await _unitOfWork.BeginTransactionAsync();

//            try
//            {
//                foreach (var item in request.Items)
//                {
//                    // 1. جلب كل الـ Batches اللي فيها كمية للمنتج ده
//                    // الترتيب هنا هو اللي بيحدد FILO (OrderByDescending CreationDate)
//                    var availableBatches = await _batchRepository.GetAvailableBatchesForProductAsync(item.ProductId, request.BrandId);
//                    // الترتيب التنازلي يضمن إن أحدث Batch هي اللي فوق
//                    availableBatches = availableBatches.OrderByDescending(b => b.CreatedDate).ToList();

//                    int remainingQuantityToPick = item.Quantity;

//                    foreach (var batch in availableBatches)
//                    {
//                        if (remainingQuantityToPick <= 0) break;

//                        int pickFromThisBatch = Math.Min(batch.CurrentQuantity, remainingQuantityToPick);

//                        // 2. تحديث كمية الـ Batch (خصم)
//                        batch.DecreaseQuantity(pickFromThisBatch);
//                        await _batchRepository.UpdateAsync(batch);

//                        // 3. تسجيل حركة المخزن (Out)
//                        var movement = new StockMovement(
//                            item.ProductId,
//                            batch.Id,
//                            batch.WarehouseId, // البيع بيتم من المخزن اللي فيه الـ Batch
//                            request.BrandId,
//                            pickFromThisBatch,
//                            StockMovementType.SalesOut,
//                            StockReferenceType.SalesInvoice,
//                            Guid.NewGuid() // ده هيكون الـ InvoiceId لاحقاً
//                        );

//                        await _movementRepository.CreateAsync(movement);

//                        remainingQuantityToPick -= pickFromThisBatch;
//                    }

//                    if (remainingQuantityToPick > 0)
//                        throw new BusinessException($"Inadequate stock for product {item.ProductId}. Missing: {remainingQuantityToPick}");
//                }

//                await _unitOfWork.SaveChangesAsync(cancellationToken);
//                await _unitOfWork.CommitTransactionAsync();
//                return true;
//            }
//            catch (Exception)
//            {
//                await _unitOfWork.RollbackTransactionAsync();
//                throw;
//            }
//        }
//    }
//}
