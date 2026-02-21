using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
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
        public string? Barcode { get; set; }
        public Stream? Image { get; set; }
        public string? ImageExtension { get; set; }
    }

    public class CreateProductCommandHandler : BaseHandler<IProductCommandRepository>, IRequestHandler<CreateProductCommand, Response<ProductDto>>
    {
        private readonly IImageStorageService _storageService;

        public CreateProductCommandHandler(IProductCommandRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork, IImageStorageService storageService)
            : base(mapper, productRepository, unitOfWork)
        {
            this._storageService = storageService;
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

            return await ExecuteCreateAsync<Domain.Entities.Products.Product, ProductDto>(
                product,
                async (p) => await _repo.CreateAsync(p),
                cancellationToken);
        }
    }
}