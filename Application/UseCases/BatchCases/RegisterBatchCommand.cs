using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using MediatR;
using AutoMapper;
using Domain.Repositories.Commands;


namespace Application.UseCases.BatchCases
{
    public class RegisterBatchCommand : IRequest<Response<BatchDto>>
    {
        public Guid ProductId { get; set; }
        public Guid PurchaseItemId { get; set; }
        public int InitialQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class RegisterBatchCommandHandler : BaseHandler<IBatchCommandRepository>, IRequestHandler<RegisterBatchCommand, Response<BatchDto>>
    {
        public RegisterBatchCommandHandler(IBatchCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<BatchDto>> Handle(RegisterBatchCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Products.Batch>(request);

            return await ExecuteCreateAsync<Domain.Entities.Products.Batch, BatchDto>(
                entity,
                async (b) => await _repo.CreateAsync(b),
                cancellationToken);
        }
    }
}