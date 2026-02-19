using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Branch.Delete
{
    public class DeleteBranchCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteBranchCommandHandler : BaseHandler<IBranchCommandRepository>, IRequestHandler<DeleteBranchCommand, Response<bool>>
    {
        public DeleteBranchCommandHandler(IBranchCommandRepository branchRepository, IUnitOfWork unitOfWork)
            : base(null, branchRepository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
        {
            var existingBranch = await _repo.GetByIdAsync(request.Id);
            if (existingBranch == null)
                return new Response<bool>(false, "Branch not found");

            return await ExecuteDeleteAsync(
                existingBranch,
                async (b) => await _repo.DeleteAsync(b),
                cancellationToken);
        }
    }
}
