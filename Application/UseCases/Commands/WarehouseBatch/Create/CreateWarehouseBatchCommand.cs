using Application.Bases;
using Application.Common.Interfaces;
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
        public CreateWarehouseBatchCommandHandler(IWarehouseBatchCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<WarehouseBatchDto>> Handle(CreateWarehouseBatchCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Products.WarehouseBatch>(request);

            return await ExecuteCreateAsync<Domain.Entities.Products.WarehouseBatch, WarehouseBatchDto>(
                entity,
                async (wb) => await _repo.CreateAsync(wb),
                cancellationToken);
        }
    }
}
