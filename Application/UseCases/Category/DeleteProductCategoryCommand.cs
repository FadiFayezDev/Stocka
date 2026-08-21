using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Products;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.Category
{
    public class DeleteProductCategoryCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }

        public DeleteProductCategoryCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteProductCategoryCommandHandler
        : BaseHandler<IProductCategoryCommandRepository>,
          IRequestHandler<DeleteProductCategoryCommand, Response<bool>>
    {
        public DeleteProductCategoryCommandHandler(
            IProductCategoryCommandRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<bool>> Handle(
            DeleteProductCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<bool>("Product category not found");

            return await ExecuteDeleteAsync<ProductCategory>(
                existing,
                async (c) => await _repo.DeleteAsync(c),
                cancellationToken);
        }
    }
}