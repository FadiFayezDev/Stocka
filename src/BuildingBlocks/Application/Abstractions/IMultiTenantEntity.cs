using System;

namespace BuildingBlocks.Application.Abstractions
{
    public interface IMultiTenantEntity
    {
        Guid BrandId { get; set; }
    }
}
