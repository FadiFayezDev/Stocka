using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.StockMovement.Create
{
    public class CreateStockMovementCommand : IRequest<Response<StockMovementDto>>
    {
        public Guid ProductId { get; set; }
        public Guid BatchId { get; set; }
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
        public string MovementType { get; set; } = null!;
        public string? ReferenceType { get; set; }
        public Guid? ReferenceId { get; set; }
    }

    public class CreateStockMovementCommandHandler : BaseHandler<IStockMovementCommandRepository>, IRequestHandler<CreateStockMovementCommand, Response<StockMovementDto>>
    {
        public CreateStockMovementCommandHandler(IStockMovementCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<StockMovementDto>> Handle(CreateStockMovementCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Products.StockMovement>(request);

            return await ExecuteCreateAsync<Domain.Entities.Products.StockMovement, StockMovementDto>(
                entity,
                async (sm) => await _repo.CreateAsync(sm),
                cancellationToken);
        }
    }
}
