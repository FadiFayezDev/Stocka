using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Queries.ProductCategory.GetById
{
    public class GetProductCategoryByIdQuery : IRequest<Response<ProductCategoryDto>>
    {
        public Guid Id { get; set; }
        public GetProductCategoryByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetProductCategoryByIdQueryHandler : BaseHandler<IProductCategoryCommandRepository>, IRequestHandler<GetProductCategoryByIdQuery, Response<ProductCategoryDto>>
    {
        public GetProductCategoryByIdQueryHandler(IProductCategoryCommandRepository productCategoryRepository, IMapper mapper) : base(mapper, productCategoryRepository)
        {
        }

        public async Task<Response<ProductCategoryDto>> Handle(GetProductCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var productCategory = await _repo.GetByIdAsync(request.Id);
            if (productCategory == null)
                return new Response<ProductCategoryDto>("ProductCategory not found");

            var productCategoryDto = _mapper.Map<ProductCategoryDto>(productCategory);
            return Success(productCategoryDto);
        }
    }
}
