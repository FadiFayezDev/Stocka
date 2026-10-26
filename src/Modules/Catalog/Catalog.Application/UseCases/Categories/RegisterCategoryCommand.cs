using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Primitives.Identifiers;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.DTOs.Category;
using Catalog.Application.Repositories.Commands;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.UseCases.Categories
{
    public record RegisterCategoryCommand : ICommand<CategoryDto?>
    {
        public string Name { get; set; } = default!;
    }

    public class RegisterCategoryCommandHandler : IRequestHandler<RegisterCategoryCommand, CategoryDto?>
    {
        private readonly ICategoryRepository _CategoryRepository;
        private readonly ITenantContext _tenantContext;

        public RegisterCategoryCommandHandler(ICategoryRepository CategoryRepository, ITenantContext tenantContext)
        {
            _CategoryRepository = CategoryRepository;
            _tenantContext = tenantContext;
        }

        public async Task<CategoryDto?> Handle(RegisterCategoryCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name is empty, name cannot be empty.", nameof(request.Name));


            var category = Category.Create(
                _tenantContext.BrandId,
                request.Name.Trim());

            await _CategoryRepository.AddAsync(category);

            return category.ToDto();
        }
    }
}