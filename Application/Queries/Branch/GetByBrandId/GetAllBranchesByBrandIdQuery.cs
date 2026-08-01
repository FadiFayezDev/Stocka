using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Branch.GetByBrandId
{
    /// <summary>
    /// The brand ID is injected automatically.
    /// </summary>
    public class GetAllBranchesByBrandIdQuery : IRequest<Response<IEnumerable<BranchDto>>>
    {

    }

    public class GetAllBranchesByBrandIdQueryHandler : IRequestHandler<GetAllBranchesByBrandIdQuery, Response<IEnumerable<BranchDto>>>
    {
        private readonly IBranchQueryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUser;

        public GetAllBranchesByBrandIdQueryHandler(IBranchQueryRepository repository, IMapper mapper, ICurrentUserContext currentUser)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<Response<IEnumerable<BranchDto>>> Handle(GetAllBranchesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var branches = await _repository.GetAllByBrandIdAsync(brandId);
            if (branches == null)
                return new Response<IEnumerable<BranchDto>>("Branches not found");

            var branchDtos = _mapper.Map<IEnumerable<BranchDto>>(branches);
            return new Response<IEnumerable<BranchDto>>(branchDtos, "Success");
        }
    }
}
