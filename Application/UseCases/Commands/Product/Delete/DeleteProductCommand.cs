using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Product.Delete
{
    public class DeleteProductCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteProductCommandHandler : BaseHandler<IProductCommandRepository>, IRequestHandler<DeleteProductCommand, Response<bool>>
    {
        public DeleteProductCommandHandler(IProductCommandRepository productRepository, IUnitOfWork unitOfWork)
            : base(null, productRepository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _repo.GetByIdAsync(request.Id);
            if (existingProduct == null)
                return new Response<bool>(false, "Product not found");

            return await ExecuteDeleteAsync(
                existingProduct,
                async (p) => await _repo.DeleteAsync(p),
                cancellationToken);
        }
    }
}
