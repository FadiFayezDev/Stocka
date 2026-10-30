using System;

namespace Catalog.Contracts.DTOs.Category
{
    /// <summary>
    /// Public Catalog module representation of a category.
    /// Carries only display identifiers (raw Guids) — never domain primitives
    /// or infrastructure concerns.
    /// </summary>
    public record CategoryDto(
        Guid Id,
        Guid OrganizationId,
        string Name,
        bool IsActive
    );
}
