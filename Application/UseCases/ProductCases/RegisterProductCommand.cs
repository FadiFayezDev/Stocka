using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ProductCases;

public sealed class RegisterProductCommand : IRequest<ProductDto>
{
    public Guid CategoryId { get; init; }

    public string Name { get; init; } = null!;

    public decimal SellingPrice { get; init; }

    public string? Barcode { get; init; }

    public Stream? Image { get; init; }

    public string? ImageExtension { get; init; }
}

public sealed class RegisterProductCommandHandler
    : IRequestHandler<RegisterProductCommand, ProductDto>
{
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IProductCommandRepository _productRepository;
    private readonly IStorageService _storageService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterProductCommandHandler> _logger;

    public RegisterProductCommandHandler(
        ICurrentUserContext currentUserContext,
        IProductCommandRepository productRepository,
        IStorageService storageService,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<RegisterProductCommandHandler> logger)
    {
        _currentUserContext = currentUserContext;
        _productRepository = productRepository;
        _storageService = storageService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ProductDto> Handle(
        RegisterProductCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var brandId = new BrandId(_currentUserContext.ActiveBrandId);

            var product = Product.Create(
                brandId,
                new ProductCategoryId(request.CategoryId),
                request.Name,
                request.SellingPrice,
                request.Barcode);

            if (request.Image is not null)
            {
                if (request.Image.Length == 0)
                    throw new BusinessException("Image is empty.");

                if (string.IsNullOrWhiteSpace(request.ImageExtension))
                    throw new BusinessException("Image extension is required.");

                var imageUrl = await _storageService.SaveAsync(
                    request.Image,
                    product.BrandId.Value,
                    product.Id.Value,
                    request.ImageExtension);

                product.ChangeImage(imageUrl);
            }

            await _productRepository.CreateAsync(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Product {ProductId} registered successfully for Brand {BrandId}.",
                product.Id.Value,
                product.BrandId.Value);

            var dto = _mapper.Map<ProductDto>(product);

            if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
            {
                dto.ImageUrl += _storageService.GetToken();
            }

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to register product '{ProductName}'.",
                request.Name);

            throw;
        }
    }
}