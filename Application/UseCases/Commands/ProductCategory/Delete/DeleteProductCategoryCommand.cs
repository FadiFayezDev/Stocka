using Application.Bases;
using Application.Dtos.Products;
using AutoMapper;
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
        public DeleteProductCategoryCommandHandler(IProductCategoryCommandRepository Repository) : base(null, Repository)
        {
        }

        public async Task<Response<bool>> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Not found");

            await _repo.DeleteAsync(existing);

            return new Response<bool>();
        }
    }
}
