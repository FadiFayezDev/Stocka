using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Warehouse.GetAll
{
    public class GetAllWarehousesQuery : IRequest<Response<IEnumerable<WarehouseDto>>>
    {
    }

    public class GetAllWarehousesQueryHandler : BaseHandler<IWarehouseQueryRepository>, IRequestHandler<GetAllWarehousesQuery, Response<IEnumerable<WarehouseDto>>>
    {
        private readonly ICurrentUserContext _currentUser;

        public GetAllWarehousesQueryHandler(IWarehouseQueryRepository warehouseRepository, ICurrentUserContext currentUser) : base(warehouseRepository)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<WarehouseDto>>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var warehouses = await _repo.GetAllByBrandIdAsync(brandId);
            return new Response<IEnumerable<WarehouseDto>>(warehouses, "Success");
        }
    }
}
