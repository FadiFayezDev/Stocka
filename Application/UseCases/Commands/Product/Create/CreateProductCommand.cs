using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Product.Create
{
    public class CreateProductCommand : IRequest<Response<ProductDto>>
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public decimal SellingPrice { get; set; }
        public string? Barcode { get; set; }
        public Stream? Image { get; set; }
        public string? ImageExtension { get; set; }
    }

    public class CreateProductCommandHandler : BaseHandler<IProductCommandRepository, IProductQueryRepository>, IRequestHandler<CreateProductCommand, Response<ProductDto>>
    {
        private readonly IStorageService _storageService;
        private readonly ICurrentUserContext _currentUser;
        public CreateProductCommandHandler(IMapper mapper, IProductCommandRepository command, IProductQueryRepository query, IUnitOfWork work, IStorageService storageService, ICurrentUserContext currentUser) 
            : base(mapper, command, query, work)
        {
            _storageService = storageService;
            _currentUser = currentUser;
        }

        public async Task<Response<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var product = Domain.Entities.Products.Product.Create(new BrandId(brandId), new ProductCategoryId(request.CategoryId), request.Name, request.SellingPrice, request.Barcode);

            if(request.Image != null && request.ImageExtension != null)
            {
                var imagePath = await _storageService.SaveAsync(
                request.Image, product.BrandId.Value, product.Id.Value, request.ImageExtension);
                product.ChangeImage(imagePath);
            }

            await ExecuteCreateAsync<Domain.Entities.Products.Product, ProductDto>(
                product,
                async (p) => await _command.CreateAsync(p),
                cancellationToken);

            var productDto = await _query.GetProductWithQuantityAsync(product.Id.Value);

            if(productDto == null)
                throw new BusinessException("Failed to create product.");
            
            return Success(productDto);
        }
    }
}