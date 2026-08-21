using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Bases;
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

    public class DiscontinueProductCommandHandler
        : BaseHandler<IProductCommandRepository>,
          IRequestHandler<DiscontinueProductCommand, Response<bool>>
    {
        public DiscontinueProductCommandHandler(
            IProductCommandRepository productRepository,
            IUnitOfWork unitOfWork)
            : base(null, productRepository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(
            DiscontinueProductCommand request,
            CancellationToken cancellationToken)
        {
            var existingProduct = await _repo.GetByIdAsync(request.Id);
            if (existingProduct == null)
                throw new BusinessException("Product not found");

            existingProduct.Deactivate();

            await _repo.UpdateAsync(existingProduct);

            var result = await _work.SaveChangesAsync(cancellationToken);
            if (result < 0)
                throw new BusinessException("Product is not saved");

            return new Response<bool>(true, "Deleted Successfully");
        }
    }
}