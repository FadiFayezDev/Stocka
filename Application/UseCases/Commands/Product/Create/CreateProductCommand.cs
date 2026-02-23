using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Product.Create
{
    public class CreateProductCommand : IRequest<Response<ProductDto>>
    {
        public Guid BrandId { get; set; }
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

        public CreateProductCommandHandler(IMapper mapper, IProductCommandRepository command, IProductQueryRepository query, IUnitOfWork work, IStorageService storageService) : base(mapper, command, query, work)
        {
            _storageService = storageService;
        }

        public async Task<Response<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Domain.Entities.Products.Product>(request);

            if(request.Image != null && request.ImageExtension != null)
            {
                var imagePath = await _storageService.SaveAsync(
                request.Image, request.BrandId, product.Id, request.ImageExtension);
                product.ImagePath = imagePath;
            }

            await ExecuteCreateAsync<Domain.Entities.Products.Product, ProductDto>(
                product,
                async (p) => await _command.CreateAsync(p),
                cancellationToken);

            var productDto = await _query.GetProductsWithQuantityAsync(product.Id);

            return Success(productDto);
        }
    }
}