using System;

namespace Domain.Bases
{
    public interface IBranchScopedEntity
    {
        Guid BranchId { get; set; }
    }
}
