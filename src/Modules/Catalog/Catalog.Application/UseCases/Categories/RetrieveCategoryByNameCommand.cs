using Catalog.Application.DTOs.Category;
using Catalog.Application.Repositories.Queries.Category;
using MediatR;

namespace Catalog.Application.UseCases.Categories
{
    public record RetrieveCategoryByNameCommand : IRequest<CategoryDto>
    {
        public string Name { get; set; } = default!;
    }

    public class RetrieveCategoryByNameCommandHandler : IRequestHandler<RetrieveCategoryByNameCommand, CategoryDto>
    {
        private readonly ICategoryQueryRepository _categoryQueryRepository;

        public RetrieveCategoryByNameCommandHandler(ICategoryQueryRepository categoryQueryRepository)
        {
            _categoryQueryRepository = categoryQueryRepository;
        }

        public async Task<CategoryDto> Handle(RetrieveCategoryByNameCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Category name is required.", nameof(request.Name));

            return await _categoryQueryRepository.GetByNameAsync(request.Name.Trim())
                ?? throw new KeyNotFoundException($"Category with name '{request.Name}' was not found.");
        }
    }
}