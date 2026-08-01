using Application.Bases;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.StockMovement.GetByBrandId
{
    public class GetAllStockMovementsByBrandIdQuery : IRequest<Response<IEnumerable<StockMovementDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllStockMovementsByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllStockMovementsByBrandIdQueryHandler : IRequestHandler<GetAllStockMovementsByBrandIdQuery, Response<IEnumerable<StockMovementDto>>>
    {
        private readonly IStockMovementQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllStockMovementsByBrandIdQueryHandler(IStockMovementQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<StockMovementDto>>> Handle(GetAllStockMovementsByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var stockMovements = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (stockMovements == null)
                return new Response<IEnumerable<StockMovementDto>>("Stock movements not found");

            var stockMovementDtos = _mapper.Map<IEnumerable<StockMovementDto>>(stockMovements);
            return new Response<IEnumerable<StockMovementDto>>(stockMovementDtos, "Success");
        }
    }
}
