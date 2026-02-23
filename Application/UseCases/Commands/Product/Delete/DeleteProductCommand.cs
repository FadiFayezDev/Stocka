using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Product.Delete
{
    public class DeleteProductCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
        public DeleteProductCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteProductCommandHandler : BaseHandler<IProductCommandRepository>, IRequestHandler<DeleteProductCommand, Response<bool>>
    {
        private readonly IStorageService _storageService;
        public DeleteProductCommandHandler(IProductCommandRepository productRepository, IUnitOfWork unitOfWork, IStorageService storageService)
            : base(null, productRepository, unitOfWork)
        {
            _storageService = storageService;
        }

        public async Task<Response<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _repo.GetByIdAsync(request.Id);
            if (existingProduct == null)
                throw new BusinessException("Product not found");

            if (!string.IsNullOrEmpty(existingProduct.ImagePath))
            {
                await _storageService.RemoveAsync(existingProduct.ImagePath);
            }

            return await ExecuteDeleteAsync(
                existingProduct,
                async (p) => await _repo.DeleteAsync(p),
                cancellationToken);
        }
    }
}
