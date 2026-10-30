using BuildingBlocks.Application.Common.Interfaces;
using Catalog.Application.Bindings.CategoryCases;
using Catalog.Application.DTOs.Category;
using Catalog.Application.Repositories.Commands;
using Catalog.Contracts.DTOs.Category;
using Catalog.Contracts.UseCases.CategoryCases;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.UseCases.Categories
{
    /// <summary>
    /// Creates the category through the domain factory (which enforces name invariants)
    /// and returns the created category data transfer object.
    /// </summary>
    public class RegisterCategoryCommandHandler : IRequestHandler<RegisterCategoryCommandMediatR, CategoryDto?>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUserContext _currentUserContext;

        public RegisterCategoryCommandHandler(ICategoryRepository categoryRepository, ICurrentUserContext currentUserContext)
        {
            _categoryRepository = categoryRepository;
            _currentUserContext = currentUserContext;
        }

        public async Task<CategoryDto?> Handle(RegisterCategoryCommandMediatR request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Contract.Name))
                throw new ArgumentException("Name is empty, name cannot be empty.", nameof(request.Contract.Name));

            var category = Category.Create(
                _currentUserContext.ActiveOrganizationId,
                request.Contract.Name.Trim());

            await _categoryRepository.AddAsync(category);

            return category.ToDto();
        }
    }
}
