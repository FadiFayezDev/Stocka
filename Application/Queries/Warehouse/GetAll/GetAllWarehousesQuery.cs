using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Warehouse.GetAll
{
    public class GetAllWarehousesQuery : IRequest<Response<IEnumerable<WarehouseDto>>>
    {
    }

    public class GetAllWarehousesQueryHandler : BaseHandler<IWarehouseCommandRepository>, IRequestHandler<GetAllWarehousesQuery, Response<IEnumerable<WarehouseDto>>>
    {
        public GetAllWarehousesQueryHandler(IWarehouseCommandRepository warehouseRepository, IMapper mapper) : base(mapper, warehouseRepository)
        {
        }

        public async Task<Response<IEnumerable<WarehouseDto>>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
        {
            var warehouses = await _repo.GetAllTableAsync();
            var warehouseDtos = _mapper.Map<IEnumerable<WarehouseDto>>(warehouses);
            return new Response<IEnumerable<WarehouseDto>>(warehouseDtos, "Success");
        }
    }
}
