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

namespace Application.Features.Commands.Product.Update
{
    public class UpdateProductCommand : IRequest<Response<ProductDto>>
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!; 
        public decimal SellingPrice { get; set; }
        public string? Barcode { get; set; }
        public Stream? Image { get; set; }
        public string? ImageExtension { get; set; }
        public bool? IsActive { get; set; }
    }

    public class UpdateProductCommandHandler : BaseHandler<IProductCommandRepository, IProductQueryRepository>, IRequestHandler<UpdateProductCommand, Response<ProductDto>>
    {
        private readonly IStorageService _storageService;

        public UpdateProductCommandHandler(IMapper mapper, IProductCommandRepository command, IProductQueryRepository query, IUnitOfWork work, IStorageService storageService) : base(mapper, command, query, work)
        {
            _storageService = storageService;
        }

        public async Task<Response<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _command.GetByIdAsync(request.Id);
            if (existingProduct == null)
                throw new BusinessException("product in not found");

            existingProduct.Rename(request.Name);
            existingProduct.ChangeSellingPrice(request.SellingPrice);
            existingProduct.ChangeBarcode(request.Barcode);
            existingProduct.ChangeCategory(new ProductCategoryId(request.CategoryId));

            if (request.IsActive.HasValue)
            {
                if (request.IsActive.Value)
                    existingProduct.Activate();
                else
                    existingProduct.Deactivate();
            }

            if (request.Image != null && request.ImageExtension != null)
            {
                var imagePath = await _storageService.SaveAsync(
                request.Image, existingProduct.BrandId.Value, existingProduct.Id.Value, request.ImageExtension);
                existingProduct.ChangeImage(imagePath);
            }

             await ExecuteUpdateAsync<Domain.Entities.Products.Product, ProductDto>(
                existingProduct,
                async (p) => await _command.UpdateAsync(p),
                cancellationToken);

            var dto = await _query.GetProductWithQuantityAsync(existingProduct.Id.Value);

            if (dto == null)
                throw new BusinessException("product in not found after update");

            return Success(dto);
        }
    }
}