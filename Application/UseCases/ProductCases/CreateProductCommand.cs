using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ProductCases
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public decimal SellingPrice { get; set; }
        public string? Barcode { get; set; }
        public Stream? Image { get; set; }
        public string? ImageExtension { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IProductCommandRepository _productCommand;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<ProductDto> _logger;
        public CreateProductCommandHandler(
            ICurrentUserContext currentUserContext,
            IProductCommandRepository productCommand,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<ProductDto> logger,
            IStorageService storageService)
        {
            _currentUserContext = currentUserContext;
            _productCommand = productCommand;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _storageService = storageService;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUserContext.ActiveBrandId;
            try
            {
                var product = Product.Create(new BrandId(brandId), new ProductCategoryId(request.CategoryId), request.Name, request.SellingPrice, request.Barcode);
            
                if (request.Image != null || request.Image?.Length == 0)
                {
                    // TODO: ImageExtension ممكن يجي Null وده ممكن يعمل مشاكل!
                    var url = await _storageService.SaveAsync(request.Image, product.BrandId.Value, product.Id.Value, request.ImageExtension);
                    product.ChangeImage(url);
                }

                if (product == null)
                    throw new BusinessException("Product not set.");

                await _productCommand.CreateAsync(product);

                var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.Log(LogLevel.Information, "Product is created");

                var dto = _mapper.Map<ProductDto>(product);

                dto.ImageUrl = _storageService.GetToken();

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Product failed");
                throw;
            }
        }
    }

}
