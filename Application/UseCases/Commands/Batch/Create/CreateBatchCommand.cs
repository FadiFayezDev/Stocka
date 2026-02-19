using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using MediatR;
using AutoMapper;
using Domain.Repositories.Commands;


namespace Application.Features.Commands.Batch.Create
{
    public class CreateBatchCommand : IRequest<Response<BatchDto>>
    {
        public Guid ProductId { get; set; }
        public Guid PurchaseItemId { get; set; }
        public int InitialQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class CreateBatchCommandHandler : BaseHandler<IBatchCommandRepository>, IRequestHandler<CreateBatchCommand, Response<BatchDto>>
    {
        public CreateBatchCommandHandler(IBatchCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<BatchDto>> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Products.Batch>(request);
            entity.CreatedAt = DateTime.UtcNow;

            return await ExecuteCreateAsync<Domain.Entities.Products.Batch, BatchDto>(
                entity,
                async (b) => await _repo.CreateAsync(b),
                cancellationToken);
        }
    }
}