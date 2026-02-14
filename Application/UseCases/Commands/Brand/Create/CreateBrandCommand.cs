using Application.Bases;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.Commands.Brand.Create
{
    public class CreateBrandCommand : IRequest<Response<BrandDto>>
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Response<BrandDto>>
    {
        private readonly IBrandCommandRepository _repository;
        private readonly IMapper _mapper;

        public CreateBrandCommandHandler(IBrandCommandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<BrandDto>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = _mapper.Map<Domain.Entities.Core.Brand>(request);
            var success = await _repository.CreateAsync(brand);
            
            if (!success)
                return new Response<BrandDto>(false, "Failed to create brand");

            var dto = _mapper.Map<BrandDto>(brand);
            return new Response<BrandDto>(dto, "Created Successfully");
        }
    }
}
