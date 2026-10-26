using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Primitives.Identifiers;
using Catalog.Application.DTOs.Category;
using Catalog.Application.Repositories.Queries.Category;
using MediatR;

namespace Catalog.Application.UseCases.Categories
{
    public record ListCategoriesCommand : IRequest<IEnumerable<CategoryDto>>
    {
        public Guid BrandId { get; set; } = Guid.Empty;
    }

    public class ListCategoriesCommandHandler : IRequestHandler<ListCategoriesCommand, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryQueryRepository _categoryQueryRepository;
        private readonly ITenantContext _tenantContext;

        public ListCategoriesCommandHandler(ICategoryQueryRepository categoryQueryRepository, ITenantContext tenantContext)
        {
            _categoryQueryRepository = categoryQueryRepository;
            _tenantContext = tenantContext;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(ListCategoriesCommand request, CancellationToken cancellationToken)
        {
            var brandId = request.BrandId == Guid.Empty
                ? _tenantContext.BrandId
                : BrandId.FromGuid(request.BrandId);

            return await _categoryQueryRepository.GetAllAsync(brandId);
        }
    }
}