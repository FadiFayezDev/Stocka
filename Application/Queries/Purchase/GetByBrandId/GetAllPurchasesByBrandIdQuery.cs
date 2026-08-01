using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Purchase.GetByBrandId
{
    public class GetAllPurchasesByBrandIdQuery : IRequest<Response<IEnumerable<PurchaseDto>>>
    {

    }

    public class GetAllPurchasesByBrandIdQueryHandler : IRequestHandler<GetAllPurchasesByBrandIdQuery, Response<IEnumerable<PurchaseDto>>>
    {
        private readonly IPurchaseQueryRepository _repository;
        private readonly ICurrentUserContext _currentUser;
        public GetAllPurchasesByBrandIdQueryHandler(IPurchaseQueryRepository repository, ICurrentUserContext currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<PurchaseDto>>> Handle(GetAllPurchasesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;


            var purchases = await _repository.GetAllByBrandIdAsync(brandId);
            if (purchases == null)
                throw new BusinessException("Purchases not found for the specified brand.");

            return new Response<IEnumerable<PurchaseDto>>(purchases, "Success");
        }
    }
}