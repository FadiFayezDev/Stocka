using Application.Bases;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Brand.GetAll
{
    public class GetAllBrandsQuery : IRequest<Response<IEnumerable<BrandDto>>>
    {
    }

    public class GetAllBrandsQueryHandler : BaseHandler<IBrandCommandRepository>, IRequestHandler<GetAllBrandsQuery, Response<IEnumerable<BrandDto>>>
    {
        public GetAllBrandsQueryHandler(IBrandCommandRepository brandRepository, IMapper mapper) : base(mapper, brandRepository)
        {
        }

        public async Task<Response<IEnumerable<BrandDto>>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _repo.GetAllTableAsync();
            var brandDtos = _mapper.Map<IEnumerable<BrandDto>>(brands);
            return new Response<IEnumerable<BrandDto>>(brandDtos, "Success");
        }
    }
}
