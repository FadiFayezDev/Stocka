using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.ProductCategory.Create
{
    public class CreateProductCategoryCommand : IRequest<Response<ProductCategoryDto>>
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class CreateProductCategoryCommandHandler : BaseHandler<IProductCategoryCommandRepository>, IRequestHandler<CreateProductCategoryCommand, Response<ProductCategoryDto>>
    {
        public CreateProductCategoryCommandHandler(IProductCategoryCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<ProductCategoryDto>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Products.ProductCategory>(request);

            return await ExecuteCreateAsync<Domain.Entities.Products.ProductCategory, ProductCategoryDto>(
                entity,
                async (pc) => await _repo.CreateAsync(pc),
                cancellationToken);
        }
    }
}
