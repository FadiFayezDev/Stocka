using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
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
        public DeleteProductCommandHandler(IProductCommandRepository productRepository) : base(null, productRepository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _repo.GetByIdAsync(request.Id);
            if (existingProduct == null)
                return new Response<bool>("Product not found");

            await _repo.DeleteAsync(existingProduct);

            return new Response<bool>();
        }
    }
}
