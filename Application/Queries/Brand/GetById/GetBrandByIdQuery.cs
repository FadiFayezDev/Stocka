using Application.Bases;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.Brand.GetById
{
    public class GetBrandByIdQuery : IRequest<Response<BrandDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetBrandByIdQueryHandler : BaseHandler<IBrandCommandRepository>, IRequestHandler<GetBrandByIdQuery, Response<BrandDto>>
    {
        public GetBrandByIdQueryHandler(IBrandCommandRepository brandRepository, IMapper mapper) : base(mapper, brandRepository)
        {
        }

        public async Task<Response<BrandDto>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brand = await _repo.GetByIdAsync(request.Id);
            if (brand == null)
                return new Response<BrandDto>("Brand not found");

            var brandDto = _mapper.Map<BrandDto>(brand);
            return new Response<BrandDto>(brandDto, "Success");
        }
    }
}
