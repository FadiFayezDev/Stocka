using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Warehouse.GetById
{
    public class GetWarehouseByIdQuery : IRequest<Response<WarehouseDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetWarehouseByIdQueryHandler : BaseHandler<IWarehouseCommandRepository>, IRequestHandler<GetWarehouseByIdQuery, Response<WarehouseDto>>
    {
        public GetWarehouseByIdQueryHandler(IWarehouseCommandRepository warehouseRepository, IMapper mapper) : base(mapper, warehouseRepository)
        {
        }

        public async Task<Response<WarehouseDto>> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
        {
            var warehouse = await _repo.GetByIdAsync(request.Id);
            if (warehouse == null)
                return new Response<WarehouseDto>("Warehouse not found");

            var warehouseDto = _mapper.Map<WarehouseDto>(warehouse);
            return new Response<WarehouseDto>(warehouseDto, "Success");
        }
    }
}
