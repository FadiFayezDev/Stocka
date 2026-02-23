using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Entities.Products;
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

        public UpdateProductCommandHandler(IMapper mapper, IProductCommandRepository command, IProductQueryRepository query, IUnitOfWork work) : base(mapper, command, query, work)
        {
        }

        public async Task<Response<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _command.GetByIdAsync(request.Id);
            if (existingProduct == null)
                throw new BusinessException("product in not found");

            var brandId = existingProduct.BrandId;

            _mapper!.Map(request, existingProduct);

            if (request.Image != null && request.ImageExtension != null)
            {
                var imagePath = await _storageService.SaveAsync(
                request.Image, brandId, existingProduct.BrandId, request.ImageExtension);
                existingProduct.ImagePath = imagePath;
            }

             await ExecuteUpdateAsync<Domain.Entities.Products.Product, ProductDto>(
                existingProduct,
                async (p) => await _command.UpdateAsync(p),
                cancellationToken);

            var dto = await _query.GetProductsWithQuantityAsync(existingProduct.Id);

            //dto.ImageUrl += _storageService.GetToken();

            return Success(dto);
        }
    }
}