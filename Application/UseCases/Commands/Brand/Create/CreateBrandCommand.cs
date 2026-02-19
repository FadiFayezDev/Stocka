using Application.Bases;
using Application.Common.Interfaces;
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

    public class CreateBrandCommandHandler : BaseHandler<IBrandCommandRepository>, IRequestHandler<CreateBrandCommand, Response<BrandDto>>
    {
        public CreateBrandCommandHandler(IBrandCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork) 
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<BrandDto>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = _mapper.Map<Domain.Entities.Core.Brand>(request);
            
            return await ExecuteCreateAsync<Domain.Entities.Core.Brand, BrandDto>(
                brand,
                async (b) => await _repo.CreateAsync(b),
                cancellationToken);
        }
    }
}
