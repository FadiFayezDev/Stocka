using Application.Bases;
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
        public CreateBatchCommandHandler(IBatchCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<BatchDto>> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Products.Batch>(request);
            entity.CreatedAt = DateTime.UtcNow;

            await _repo.CreateAsync(entity);

            return new Response<BatchDto>();
        }
    }
}