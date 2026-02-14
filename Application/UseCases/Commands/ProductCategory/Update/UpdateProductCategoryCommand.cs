using Application.Bases;
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
        public UpdateProductCategoryCommandHandler(IProductCategoryCommandRepository Repository, IMapper mapper) : base(mapper, Repository)
        {
        }

        public async Task<Response<ProductCategoryDto>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<ProductCategoryDto>("Not found");

            _mapper.Map(request, existing);

             await _repo.UpdateAsync(existing);

            return new Response<ProductCategoryDto>();
        }
    }
}
