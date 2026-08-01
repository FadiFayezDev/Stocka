using Application.Bases;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Warehouse.GetByBrandId
{
    public class GetAllWarehousesByBrandIdQuery : IRequest<Response<IEnumerable<WarehouseDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllWarehousesByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllWarehousesByBrandIdQueryHandler : IRequestHandler<GetAllWarehousesByBrandIdQuery, Response<IEnumerable<WarehouseDto>>>
    {
        private readonly IWarehouseQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllWarehousesByBrandIdQueryHandler(IWarehouseQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<WarehouseDto>>> Handle(GetAllWarehousesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var warehouses = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (warehouses == null)
                return new Response<IEnumerable<WarehouseDto>>("Warehouses not found");

            var warehouseDtos = _mapper.Map<IEnumerable<WarehouseDto>>(warehouses);
            return new Response<IEnumerable<WarehouseDto>>(warehouseDtos, "Success");
        }
    }
}
