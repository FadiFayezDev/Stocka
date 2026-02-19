using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Batch.Update
{
    public class UpdateBatchCommand : IRequest<Response<BatchDto>>
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid PurchaseItemId { get; set; }
        public int InitialQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class UpdateBatchCommandHandler : BaseHandler<IBatchCommandRepository>, IRequestHandler<UpdateBatchCommand, Response<BatchDto>>
    {
        public UpdateBatchCommandHandler(IBatchCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<BatchDto>> Handle(UpdateBatchCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<BatchDto>("Batch not found");

            _mapper.Map(request, existing);

            return await ExecuteUpdateAsync<Domain.Entities.Products.Batch, BatchDto>(
                existing,
                async (b) => await _repo.UpdateAsync(b),
                cancellationToken);
        }
    }
}