using BuildingBlocks.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Application.Common.Interfaces
{
    public interface ICurrentUserContext
    {
        Guid UserId { get; }
        Guid ActiveOrganizationId { get; }
        Guid? ActiveBranchId { get; }
        bool IsAuthenticated { get; }
        OrganizationRole Role { get; }
        bool IsOwner { get; }
        bool CanAccessAllBranches { get; }
    }}
