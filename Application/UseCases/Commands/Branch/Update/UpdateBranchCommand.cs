using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Branch.Update
{
    public class UpdateBranchCommand : IRequest<Response<BranchDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class UpdateBranchCommandHandler : BaseHandler<IBranchCommandRepository>, IRequestHandler<UpdateBranchCommand, Response<BranchDto>>
    {
        public UpdateBranchCommandHandler(IBranchCommandRepository branchRepository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, branchRepository, unitOfWork)
        {
        }

        public async Task<Response<BranchDto>> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
        {
            var existingBranch = await _repo.GetByIdAsync(request.Id);
            if (existingBranch == null)
                throw new BusinessException("Branch not found");

            existingBranch.UpdateName(request.Name);

            return await ExecuteUpdateAsync<Domain.Entities.Core.Branch, BranchDto>(
                existingBranch,
                async (b) => await _repo.UpdateAsync(b),
                cancellationToken);
        }
    }
}
