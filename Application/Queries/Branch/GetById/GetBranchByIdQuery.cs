using Application.Bases;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Branch.GetById
{
    public class GetBranchByIdQuery : IRequest<Response<BranchDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetBranchByIdQueryHandler : BaseHandler<IBranchCommandRepository>, IRequestHandler<GetBranchByIdQuery, Response<BranchDto>>
    {
        public GetBranchByIdQueryHandler(IBranchCommandRepository branchRepository, IMapper mapper) : base(mapper, branchRepository)
        {
        }

        public async Task<Response<BranchDto>> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
        {
            var branch = await _repo.GetByIdAsync(request.Id);
            if (branch == null)
                return new Response<BranchDto>("Branch not found");

            var branchDto = _mapper.Map<BranchDto>(branch);
            return new Response<BranchDto>(branchDto, "Success");
        }
    }
}
