using Application.Common.Exceptions;
using Application.Dtos.Core;
using Application.QueryRepositories;
using MediatR;

namespace Application.UseCases.BranchCases
{
    public class RetrieveBranchCommand : IRequest<BranchDto>
    {
        public Guid BranchId { get; set; }

        public RetrieveBranchCommand(Guid branchId)
        {
            BranchId = branchId;
        }
    }

    public class RetrieveBranchCommandHandler : IRequestHandler<RetrieveBranchCommand, BranchDto>
    {
        private readonly IBranchQueryRepository _branchQuery;

        public RetrieveBranchCommandHandler(IBranchQueryRepository brandQuery)
        {
            _branchQuery = brandQuery;
        }

        public async Task<BranchDto> Handle(RetrieveBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = await _branchQuery.GetByIdAsync(request.BranchId);
            if (branch == null)
                throw new BusinessException("Branch is not found.");
            return branch;
        }
    }
}