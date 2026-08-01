using Application.Bases;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.WarehouseBatch.GetByBrandId
{
    public class GetAllWarehouseBatchesByBrandIdQuery : IRequest<Response<IEnumerable<WarehouseBatchDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllWarehouseBatchesByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllWarehouseBatchesByBrandIdQueryHandler : IRequestHandler<GetAllWarehouseBatchesByBrandIdQuery, Response<IEnumerable<WarehouseBatchDto>>>
    {
        private readonly IWarehouseBatchQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllWarehouseBatchesByBrandIdQueryHandler(IWarehouseBatchQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<WarehouseBatchDto>>> Handle(GetAllWarehouseBatchesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var warehouseBatches = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (warehouseBatches == null)
                return new Response<IEnumerable<WarehouseBatchDto>>("Warehouse batches not found");

            var warehouseBatchDtos = _mapper.Map<IEnumerable<WarehouseBatchDto>>(warehouseBatches);
            return new Response<IEnumerable<WarehouseBatchDto>>(warehouseBatchDtos, "Success");
        }
    }
}
