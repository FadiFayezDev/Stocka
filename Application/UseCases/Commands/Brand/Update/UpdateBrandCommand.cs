using Application.Bases;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.Commands.Brand.Update
{
    public class UpdateBrandCommand : IRequest<Response<BrandDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }

    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Response<BrandDto>>
    {
        private readonly IBrandCommandRepository _repository;
        private readonly IMapper _mapper;

        public UpdateBrandCommandHandler(IBrandCommandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<BrandDto>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var existingBrand = await _repository.GetByIdAsync(request.Id);
            if (existingBrand == null)
                return new Response<BrandDto>("Not found");

            _mapper.Map(request, existingBrand);
            var success = await _repository.UpdateAsync(existingBrand);

            if (!success)
                return new Response<BrandDto>(false, "Failed to update brand");

            var dto = _mapper.Map<BrandDto>(existingBrand);
            return new Response<BrandDto>(dto, "Updated Successfully");
        }
    }
}
