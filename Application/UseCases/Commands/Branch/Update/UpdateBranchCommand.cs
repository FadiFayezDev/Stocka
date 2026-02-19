using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using AutoMapper;
using MediatR;
using Domain.Repositories.Commands;

namespace Application.Features.Commands.Branch.Update
{
    public class UpdateBranchCommand : IRequest<Response<BranchDto>>
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
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
                return new Response<BranchDto>("Branch not found");

            _mapper.Map(request, existingBranch);

            return await ExecuteUpdateAsync<Domain.Entities.Core.Branch, BranchDto>(
                existingBranch,
                async (b) => await _repo.UpdateAsync(b),
                cancellationToken);
        }
    }
}
