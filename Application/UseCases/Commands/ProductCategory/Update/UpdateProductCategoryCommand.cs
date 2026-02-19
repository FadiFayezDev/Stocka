using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.ProductCategory.Update
{
    public class UpdateProductCategoryCommand : IRequest<Response<ProductCategoryDto>>
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class UpdateProductCategoryCommandHandler : BaseHandler<IProductCategoryCommandRepository>, IRequestHandler<UpdateProductCategoryCommand, Response<ProductCategoryDto>>
    {
        public UpdateProductCategoryCommandHandler(IProductCategoryCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<ProductCategoryDto>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<ProductCategoryDto>("Product category not found");

            _mapper.Map(request, existing);

            return await ExecuteUpdateAsync<Domain.Entities.Products.ProductCategory, ProductCategoryDto>(
                existing,
                async (pc) => await _repo.UpdateAsync(pc),
                cancellationToken);
        }
    }
}
