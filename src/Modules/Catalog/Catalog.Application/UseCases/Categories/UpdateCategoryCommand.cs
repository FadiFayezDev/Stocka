using Catalog.Application.Common.Interfaces;
using Catalog.Application.DTOs.Category;
using Catalog.Application.Repositories.Commands;
using Catalog.Domain.Entities;
using Catalog.Domain.Primitives;
using MediatR;

namespace Catalog.Application.UseCases.Categories
{
    public record UpdateCategoryCommand : ICommand<CategoryDto>
    {
        public Guid CategoryId { get; set; }
        public string? Name { get; set; }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.CategoryId == Guid.Empty)
                throw new ArgumentException("Category ID is required.", nameof(request.CategoryId));

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("A category name is required.", nameof(request.Name));

            var category = await _categoryRepository.GetByIdAsync(CategoryId.FromGuid(request.CategoryId))
                ?? throw new KeyNotFoundException($"Category with id '{request.CategoryId}' was not found.");

            category.UpdateName(request.Name);

            await _categoryRepository.UpdateAsync(category);

            return category.ToDto();
        }
    }
}