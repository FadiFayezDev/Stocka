using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.StockMovement.Update
{
    public class UpdateStockMovementCommand : IRequest<Response<StockMovementDto>>
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid BatchId { get; set; }
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
        public string MovementType { get; set; } = null!;
        public string? ReferenceType { get; set; }
        public Guid? ReferenceId { get; set; }
    }

    public class UpdateStockMovementCommandHandler : BaseHandler<IStockMovementCommandRepository>, IRequestHandler<UpdateStockMovementCommand, Response<StockMovementDto>>
    {
        public UpdateStockMovementCommandHandler(IStockMovementCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<StockMovementDto>> Handle(UpdateStockMovementCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<StockMovementDto>("Not found");

            _mapper.Map(request, existing);

            await _repo.UpdateAsync(existing);

            return new Response<StockMovementDto>();
        }
    }
}
