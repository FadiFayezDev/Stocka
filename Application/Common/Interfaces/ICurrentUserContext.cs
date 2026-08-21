using System;
using Domain.Enums;

namespace Application.Common.Interfaces
{
    public interface ICurrentUserContext
    {
        Guid UserId { get; }
        Guid ActiveBrandId { get; }
        Guid? ActiveBranchId { get; }
        bool IsAuthenticated { get; }
        BrandRole Role { get; }
        bool IsOwner { get; }
        bool CanAccessAllBranches { get; }
    }
}
