using Application.Bases;
using Application.Dtos.Core;
using Application.QueryRepositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Branch.GetByBrandId
{
    public class GetAllBranchesByBrandIdQuery : IRequest<Response<IEnumerable<BranchDto>>>
    {
        public Guid BrandId { get; set; }

        public GetAllBranchesByBrandIdQuery(Guid brandId)
        {
            BrandId = brandId;
        }
    }

    public class GetAllBranchesByBrandIdQueryHandler : IRequestHandler<GetAllBranchesByBrandIdQuery, Response<IEnumerable<BranchDto>>>
    {
        private readonly IBranchQueryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllBranchesByBrandIdQueryHandler(IBranchQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<BranchDto>>> Handle(GetAllBranchesByBrandIdQuery request, CancellationToken cancellationToken)
        {
            var branches = await _repository.GetAllByBrandIdAsync(request.BrandId);
            if (branches == null)
                return new Response<IEnumerable<BranchDto>>("Branches not found");

            var branchDtos = _mapper.Map<IEnumerable<BranchDto>>(branches);
            return new Response<IEnumerable<BranchDto>>(branchDtos, "Success");
        }
    }
}
