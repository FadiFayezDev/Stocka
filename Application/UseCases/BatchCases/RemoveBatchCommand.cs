using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.BatchCases
{
    public class RemoveBatchCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class RemoveBatchCommandHandler : BaseHandler<IBatchCommandRepository>, IRequestHandler<RemoveBatchCommand, Response<bool>>
    {
        public RemoveBatchCommandHandler(IMapper mapper, IBatchCommandRepository repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(RemoveBatchCommand request, CancellationToken cancellationToken)
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