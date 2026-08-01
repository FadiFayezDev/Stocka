using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Batch.Delete
{
    public class DeleteBatchCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteBatchCommandHandler : BaseHandler<IBatchCommandRepository>, IRequestHandler<DeleteBatchCommand, Response<bool>>
    {
        public DeleteBatchCommandHandler(IMapper mapper, IBatchCommandRepository repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteBatchCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                throw new BusinessException("Batch not found");

            return await ExecuteDeleteAsync(
                existing,
                async (b) => await _repo.DeleteAsync(b),
                cancellationToken);
        }
    }
}