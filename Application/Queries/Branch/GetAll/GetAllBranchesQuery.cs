using Application.Bases;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Branch.GetAll
{
    public class GetAllBranchesQuery : IRequest<Response<IEnumerable<BranchDto>>>
    {
    }

    public class GetAllBranchesQueryHandler : BaseHandler<IBranchCommandRepository>, IRequestHandler<GetAllBranchesQuery, Response<IEnumerable<BranchDto>>>
    {
        public GetAllBranchesQueryHandler(IBranchCommandRepository branchRepository, IMapper mapper) : base(mapper, branchRepository)
        {
        }

        public async Task<Response<IEnumerable<BranchDto>>> Handle(GetAllBranchesQuery request, CancellationToken cancellationToken)
        {
            var branches = await _repo.GetAllTableAsync();
            var branchDtos = _mapper.Map<IEnumerable<BranchDto>>(branches);
            return new Response<IEnumerable<BranchDto>>(branchDtos, "Success");
        }
    }
}
