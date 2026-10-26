using Catalog.Application.Common.Interfaces;
using Catalog.Application.DTOs.Category;
using Catalog.Application.Repositories.Commands;
using Catalog.Domain.Entities;
using Catalog.Domain.Primitives;
using MediatR;

namespace Catalog.Application.UseCases.Categories
{
    public record DeactivateCategoryCommand : ICommand<CategoryDto>
    {
        public Guid CategoryId { get; set; }
    }

    public class DeactivateCategoryCommandHandler : IRequestHandler<DeactivateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeactivateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> Handle(DeactivateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.CategoryId == Guid.Empty)
                throw new ArgumentException("Category ID is required.", nameof(request.CategoryId));

            var category = await _categoryRepository.GetByIdAsync(CategoryId.FromGuid(request.CategoryId))
                ?? throw new KeyNotFoundException($"Category with id '{request.CategoryId}' was not found.");

            category.UpdateActive(false);

            await _categoryRepository.UpdateAsync(category);

            return category.ToDto();
        }
    }
}