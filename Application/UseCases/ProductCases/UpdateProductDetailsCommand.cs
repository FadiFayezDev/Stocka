using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.ProductCases
{
    public class UpdateProductDetailsCommand : IRequest<ProductDto>
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Name { get; set; } = null!;
        public decimal? SellingPrice { get; set; }
        public string? Barcode { get; set; }
        public Stream? Image { get; set; }
        public string? ImageExtension { get; set; }
        public bool? IsActive { get; set; }
    }

    public class UpdateProductDetailsCommandHandler : IRequestHandler<UpdateProductDetailsCommand, ProductDto>
    {
        private readonly IProductCommandRepository _productCommand;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProductDetailsCommandHandler(IProductCommandRepository branchCommand, IMapper mapper, IUnitOfWork unitOfWork, IStorageService storageService, IProductQueryRepository productQuery)
        {
            _productCommand = branchCommand;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }

        public async Task<ProductDto> Handle(UpdateProductDetailsCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _productCommand.GetByIdAsync(request.Id);

            if (existingEntity == null)
                throw new BusinessException("Branch is not found.");

            if (request.CategoryId != null)
                existingEntity.ChangeCategory(new ProductCategoryId(request.CategoryId.Value));

            if (request.Name != null)
                existingEntity.Rename(request.Name);

            if (request.SellingPrice.HasValue)
                existingEntity.ChangeSellingPrice(request.SellingPrice.Value);

            if (request.Barcode != null)
                existingEntity.ChangeBarcode(request.Barcode);

            if (request.IsActive.HasValue)
            {
                if (request.IsActive.Value)
                    existingEntity.Activate();
                else
                    existingEntity.Deactivate();
            }

            if (request.Image != null || request.Image?.Length == 0)
            {
                // TODO: ImageExtension ممكن يجي Null وده ممكن يعمل مشاكل!
                var url = await _storageService.SaveAsync(request.Image, existingEntity.BrandId.Value, existingEntity.Id.Value, request.ImageExtension);
                existingEntity.ChangeImage(url);
            }

            await _productCommand.UpdateAsync(existingEntity);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result < 0)
                throw new BusinessException("Product is not saved");

            // Note: It's not the best for performance 
            var product = _mapper.Map<ProductDto>(existingEntity);

            product.ImageUrl += _storageService.GetToken();

            return product;
        }
    }
}
