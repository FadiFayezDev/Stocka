using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;
using PurchaseEntity = Domain.Entities.Purchasing.Purchase;

namespace Application.UseCases.Purchase
{
    public class CreatePurchaseOrderCommand : IRequest<PurchaseWithItemsDto>
    {
        public Guid SupplierId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public List<PurchaseOrderLineDto> Lines { get; set; } = new();
    }

    public class PurchaseOrderLineDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, PurchaseWithItemsDto>
    {
        private readonly IPurchaseCommandRepository _purchaseRepo;
        private readonly ICurrentUserContext _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePurchaseOrderCommandHandler(
            IPurchaseCommandRepository purchaseRepo,
            ICurrentUserContext currentUser,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _purchaseRepo = purchaseRepo;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PurchaseWithItemsDto> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;
            if (brandId == Guid.Empty)
                throw new UnauthorizedAccessException("لا يوجد نشاط تجاري مفعّل.");

            if (!request.Lines.Any())
                throw new BadRequestException("يجب إضافة صنف واحد على الأقل لأمر الشراء.");

            if (request.Lines.Any(l => l.Quantity <= 0))
                throw new BadRequestException("كمية الصنف يجب أن تكون أكبر من صفر.");

            if (request.Lines.Any(l => l.UnitCost <= 0))
                throw new BadRequestException("سعر التكلفة يجب أن يكون أكبر من صفر.");

            var purchaseDate = request.PurchaseDate.Kind == DateTimeKind.Utc
                ? request.PurchaseDate
                : DateTime.SpecifyKind(request.PurchaseDate, DateTimeKind.Utc);

            var purchase = new PurchaseEntity(
                new BrandId(brandId),
                new SupplierId(request.SupplierId),
                purchaseDate,
                _currentUser.ActiveBranchId.HasValue ? new BranchId(_currentUser.ActiveBranchId.Value) : null);

            foreach (var line in request.Lines)
                purchase.AddPurchaseItem(new ProductId(line.ProductId), line.Quantity, line.UnitCost);

            await _purchaseRepo.CreateAsync(purchase);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PurchaseWithItemsDto>(purchase);
        }
    }
}