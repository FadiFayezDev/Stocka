using System;

namespace Domain.Bases
{
    public interface IMultiTenantEntity
    {
        Guid BrandId { get; set; }
    }
}
