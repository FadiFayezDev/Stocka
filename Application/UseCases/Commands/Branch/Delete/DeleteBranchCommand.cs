using Application.Bases;
using Application.Dtos.Core;
using AutoMapper;
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
        public DeleteBranchCommandHandler(IBranchCommandRepository branchRepository) : base(null, branchRepository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
        {
            var existingBranch = await _repo.GetByIdAsync(request.Id);
            if (existingBranch == null)
                return new Response<bool>("Branch not found");

            await _repo.DeleteAsync(existingBranch);

            return new Response<bool>();
        }
    }
}
