using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.ProductCategory.Delete
{
    public class DeleteProductCategoryCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
        public DeleteProductCategoryCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteProductCategoryCommandHandler : BaseHandler<IProductCategoryCommandRepository>, IRequestHandler<DeleteProductCategoryCommand, Response<bool>>
    {
        public DeleteProductCategoryCommandHandler(IProductCategoryCommandRepository repository, IUnitOfWork unitOfWork)
            : base(null, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>(false, "Product category not found");

            return await ExecuteDeleteAsync(
                existing,
                async (pc) => await _repo.DeleteAsync(pc),
                cancellationToken);
        }
    }
}
