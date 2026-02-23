using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Repositories.Commands;
using Domain.Repositories.Commands.Base;
using MediatR;

namespace Application.UseCases.Commands.Product.PartialUpdate
{
    public class PartialUpdateProductCommand : IRequest<Response<ProductDto>>
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Name { get; set; }
        public decimal? SellingPrice { get; set; }
        public string? Barcode { get; set; }
        public Stream? Image { get; set; }
        public string? ImageExtension { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PartialUpdateProductCommandHandler : BaseHandler<IProductCommandRepository, IProductQueryRepository>, IRequestHandler<PartialUpdateProductCommand, Response<ProductDto>>
    {
        private readonly IStorageService _storageService;

        public PartialUpdateProductCommandHandler(IMapper mapper, IProductCommandRepository commandRepository, IProductQueryRepository queryRepository, IUnitOfWork work, IStorageService storageService) : base(mapper, commandRepository, queryRepository, work)
        {
            _storageService = storageService;
        }

        public async Task<Response<ProductDto>> Handle(PartialUpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _command.GetByIdAsync(request.Id);

            if (product == null)
                throw new BusinessException("Product not found");

            if (request.CategoryId.HasValue)
                product.CategoryId = request.CategoryId.Value;

            if (!string.IsNullOrEmpty(request.Name))
                product.UpdateName(request.Name);

            if (request.SellingPrice.HasValue)
                product.UpdateSellingPrice(request.SellingPrice.Value);

            if (!string.IsNullOrEmpty(request.Barcode))
                product.UpdateBarcode(request.Barcode);

            if (request.Image != null && !string.IsNullOrEmpty(request.ImageExtension))
            {
                // Handle image saving logic here and set product.ImagePath accordingly
                product.ImagePath = await _storageService.SaveAsync(request.Image, product.BrandId, product.Id, request.ImageExtension);
            }

            if (request.IsActive.HasValue)
                product.IsActive = request.IsActive.Value;

              await ExecuteUpdateAsync<Domain.Entities.Products.Product, ProductDto>(
                product,
                async (p) => await _command.UpdateAsync(p),
                cancellationToken);

            var dto = await _query.GetProductsWithQuantityAsync(product.Id);

            //dto.ImageUrl += _storageService.GetToken();

            return Success(dto);
        }
    }
}