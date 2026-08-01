using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using MediatR;

namespace Application.UseCases.WarehouseCases
{
    public class ListWarehousesCommand : IRequest<List<WarehouseDto>>
    {
    }

    public class ListWarehousesCommandHandler : IRequestHandler<ListWarehousesCommand, List<WarehouseDto>>
    {
        private readonly IWarehouseQueryRepository _warehouseQuery;
        private readonly ICurrentUserContext _userContext;

        public ListWarehousesCommandHandler(IWarehouseQueryRepository warehouseQuery, ICurrentUserContext userContext)
        {
            _warehouseQuery = warehouseQuery;
            _userContext = userContext;
        }

        public async Task<List<WarehouseDto>> Handle(ListWarehousesCommand request, CancellationToken cancellationToken)
        {
            var brandId = _userContext.ActiveBrandId;
            var warehouses = await _warehouseQuery.GetAllByBrandIdAsync(brandId);
            return warehouses.ToList();
        }
    }
}
