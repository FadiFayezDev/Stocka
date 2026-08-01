using Application.Common.Exceptions;
using Application.Dtos.Products;
using Application.QueryRepositories;
using MediatR;

namespace Application.UseCases.WarehouseCases
{
    public class RetrieveWarehouseCommand : IRequest<WarehouseDto>
    {
        public Guid Id { get; set; }
        
        public RetrieveWarehouseCommand(Guid id)
        {
            Id = id;
        }
    }

    public class RetrieveWarehouseCommandHandler : IRequestHandler<RetrieveWarehouseCommand, WarehouseDto>
    {
        private readonly IWarehouseQueryRepository _warehouseQuery;

        public RetrieveWarehouseCommandHandler(IWarehouseQueryRepository warehouseQuery)
        {
            _warehouseQuery = warehouseQuery;
        }

        public async Task<WarehouseDto> Handle(RetrieveWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await _warehouseQuery.GetByIdAsync(request.Id);
            if (warehouse == null)
            {
                throw new BusinessException("Warehouse not found");
            }
            return warehouse;
        }
    }
}
