using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.ProductCases
{
    public class DiscontinueProductCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
        public DiscontinueProductCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DiscontinueProductCommandHandler : BaseHandler<IProductCommandRepository>, IRequestHandler<DiscontinueProductCommand, Response<bool>>
    {
        private readonly IStorageService _storageService;
        public DiscontinueProductCommandHandler(IProductCommandRepository productRepository, IUnitOfWork unitOfWork, IStorageService storageService)
            : base(null, productRepository, unitOfWork)
        {
            _storageService = storageService;
        }

        public async Task<Response<bool>> Handle(DiscontinueProductCommand request, CancellationToken cancellationToken)
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
