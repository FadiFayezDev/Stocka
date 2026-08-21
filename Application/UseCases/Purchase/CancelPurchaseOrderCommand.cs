using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.Purchase
{
    public class CancelPurchaseOrderCommand : IRequest<PurchaseWithItemsDto>
    {
        public Guid Id { get; set; }
    }

    public class CancelPurchaseOrderCommandHandler : IRequestHandler<CancelPurchaseOrderCommand, PurchaseWithItemsDto>
    {
        private readonly IPurchaseCommandRepository _purchaseRepo;
        private readonly ICurrentUserContext _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CancelPurchaseOrderCommandHandler(
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

        public async Task<PurchaseWithItemsDto> Handle(CancelPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var purchase = await _purchaseRepo.GetByIdAsync(request.Id);
            if (purchase == null || purchase.BrandId.Value != brandId)
                throw new NotFoundException("أمر الشراء المطلوب غير موجود.");

            purchase.Cancel();
            await _purchaseRepo.UpdateAsync(purchase);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PurchaseWithItemsDto>(purchase);
        }
    }
}