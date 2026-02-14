using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.WarehouseBatch.Create
{
    public class CreateWarehouseBatchCommand : IRequest<Response<WarehouseBatchDto>>
    {
        public Guid WarehouseId { get; set; }
        public Guid BatchId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateWarehouseBatchCommandHandler : BaseHandler<IWarehouseBatchCommandRepository>, IRequestHandler<CreateWarehouseBatchCommand, Response<WarehouseBatchDto>>
    {
        public CreateWarehouseBatchCommandHandler(IWarehouseBatchCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<WarehouseBatchDto>> Handle(CreateWarehouseBatchCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Products.WarehouseBatch>(request);

            await _repo.CreateAsync(entity);

            return new Response<WarehouseBatchDto>();
        }
    }
}
