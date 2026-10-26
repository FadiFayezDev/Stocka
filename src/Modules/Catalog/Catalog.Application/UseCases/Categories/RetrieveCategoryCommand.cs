using Catalog.Application.DTOs.Category;
using Catalog.Application.Repositories.Queries.Category;
using MediatR;

namespace Catalog.Application.UseCases.Categories
{
    public record RetrieveCategoryCommand : IRequest<CategoryDto>
    {
        public Guid CategoryId { get; set; }
    }

    public class RetrieveCategoryCommandHandler : IRequestHandler<RetrieveCategoryCommand, CategoryDto>
    {
        private readonly ICategoryQueryRepository _categoryQueryRepository;

        public RetrieveCategoryCommandHandler(ICategoryQueryRepository categoryQueryRepository)
        {
            _categoryQueryRepository = categoryQueryRepository;
        }

        public async Task<CategoryDto> Handle(RetrieveCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.CategoryId == Guid.Empty)
                throw new ArgumentException("Category ID is required.", nameof(request.CategoryId));

            return await _categoryQueryRepository.GetByIdAsync(request.CategoryId)
                ?? throw new KeyNotFoundException($"Category with id '{request.CategoryId}' was not found.");
        }
    }
}