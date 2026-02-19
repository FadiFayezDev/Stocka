using Application.Bases;
using Application.Common.Interfaces;
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

    public class UpdateBrandCommandHandler : BaseHandler<IBrandCommandRepository>, IRequestHandler<UpdateBrandCommand, Response<BrandDto>>
    {
        public UpdateBrandCommandHandler(IBrandCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<BrandDto>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var existingBrand = await _repo.GetByIdAsync(request.Id);
            if (existingBrand == null)
                return new Response<BrandDto>("Brand not found");

            _mapper.Map(request, existingBrand);
            
            return await ExecuteUpdateAsync<Domain.Entities.Core.Brand, BrandDto>(
                existingBrand,
                async (b) => await _repo.UpdateAsync(b),
                cancellationToken);
        }
    }
}
