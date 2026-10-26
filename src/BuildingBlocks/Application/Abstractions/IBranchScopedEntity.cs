using System;

namespace BuildingBlocks.Application.Abstractions
{
    public interface IBranchScopedEntity
    {
        Guid BranchId { get; set; }
    }
}
