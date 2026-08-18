using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.BranchCases
{
    public class RemoveBranchCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class RemoveBranchCommandHandler : BaseHandler<IBranchCommandRepository>, IRequestHandler<RemoveBranchCommand, Response<bool>>
    {
        public RemoveBranchCommandHandler(IBranchCommandRepository branchRepository, IUnitOfWork unitOfWork)
            : base(null, branchRepository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(RemoveBranchCommand request, CancellationToken cancellationToken)
        {
            var existingBranch = await _repo.GetByIdAsync(request.Id);
            if (existingBranch == null)
                throw new BusinessException("Branch not found");

            return await ExecuteDeleteAsync(
                existingBranch,
                async (b) => await _repo.DeleteAsync(b),
                cancellationToken);
        }
    }
}
