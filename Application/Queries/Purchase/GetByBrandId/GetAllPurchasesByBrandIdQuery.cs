using Application.Bases;
using Application.Dtos.Purchasing;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Purchase.GetByBrandId
{
    public class GetAllPurchasesByBrandIdQuery : IRequest<Response<IEnumerable<PurchaseDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllPurchasesByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllPurchasesByBrandIdQueryHandler : IRequestHandler<GetAllPurchasesByBrandIdQuery, Response<IEnumerable<PurchaseDto>>>
    {
        private readonly IPurchaseQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPurchasesByBrandIdQueryHandler(IPurchaseQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<PurchaseDto>>> Handle(GetAllPurchasesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var purchases = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (purchases == null)
                return new Response<IEnumerable<PurchaseDto>>("Purchases not found");

            var purchaseDtos = _mapper.Map<IEnumerable<PurchaseDto>>(purchases);
            return new Response<IEnumerable<PurchaseDto>>(purchaseDtos, "Success");
        }
    }
}
