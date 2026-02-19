# ?? COMPLETE NAMESPACE REORGANIZATION GUIDE

## Executive Summary

Fixed the namespace inconsistencies in the Stocka system. The codebase had mixed namespaces using:
- `CleanArchitecture.Application.*` (should be `Application.*`)
- `Authentication.Application.*` (should be `Application.Features.*`)
- `Application.UseCases.*` (should be `Application.Features.*`)

**Status**: ? **Phase 1 Complete** - Core fixes applied
**Next**: Phase 2 - Full consolidation to `Application.Features`

---

## ?? What's Been Fixed

### 1. Application/Results/Response.cs ?
```csharp
// ? BEFORE
namespace CleanArchitecture.Application.Bases

// ? AFTER
namespace Application.Bases
```

### 2. API/Program.cs ?
```csharp
// ? BEFORE
using CleanArchitecture.Api.Middleware;

// ? AFTER
using API.Middleware;
```

---

## ?? Namespace Analysis

### Current Issues by Severity

#### ?? CRITICAL - Cleanup Required

**File Locations**: Authentication.Application.*
```
- Authentication.Application.Commands.Auth.*
- Authentication.Application.Commands.User.*
- Authentication.Application.Commands.Role.*
- Authentication.Application.DTOs
- Authentication.Application.Queries.User.*
- Authentication.Application.Queries.Role.*
```

**Target**: Move to `Application.Features.*`

---

#### ?? HIGH PRIORITY - Standardize

**File Locations**: Mixed command namespaces
```
- Application.UseCases.Commands.* (40 commands)
- Application.Features.Commands.* (inconsistent)
- Should all be: Application.Features.*
```

**Issue**: Commands in different namespace prefixes

---

#### ?? MEDIUM - Organize

**File Locations**: Query namespaces
```
- Application.Queries.* (20 queries)
- Should be: Application.Features.*
```

**Issue**: Queries not under Features folder

---

#### ?? LOW - Consolidate

**File Locations**: DTOs scattered
```
- Application.Dtos (some files)
- Application.DTOs (other files)
- Application.Dtos.Core
- Application.Dtos.Accounting
- Should be consistent
```

**Issue**: Naming inconsistency (Dtos vs DTOs)

---

## ?? Standard Namespace Convention

### New Format
```
[Project].[Layer].[Feature].[Subfolder].[Component]

Pattern:
- Project: Domain, Application, Infrastructure, API
- Layer: Features, DTOs, Interfaces, Repositories
- Feature: Brand, Product, Account, Auth, User, etc.
- Subfolder: Commands, Queries, Create, Update, Delete
- Component: Command/Query class name
```

### Examples

#### Commands
```csharp
namespace Application.Features.Brand.Commands.Create
{
    public class CreateBrandCommand : IRequest<Response<BrandDto>>
}

namespace Application.Features.Product.Commands.Update
{
    public class UpdateProductCommand : IRequest<Response<ProductDto>>
}
```

#### Queries
```csharp
namespace Application.Features.Brand.Queries.GetAll
{
    public class GetAllBrandsQuery : IRequest<Response<IEnumerable<BrandDto>>>
}

namespace Application.Features.Account.Queries.GetById
{
    public class GetAccountByIdQuery : IRequest<Response<AccountDto>>
}
```

#### DTOs
```csharp
namespace Application.DTOs.Core
{
    public class BrandDto { }
}

namespace Application.DTOs.Accounting
{
    public class AccountDto { }
}
```

#### Controllers
```csharp
namespace API.Controllers
{
    public class BrandsController : BaseController { }
}
```

#### Repositories
```csharp
namespace Infrastructure.Repositories.Commands
{
    public class BrandCommandRepository : CommandRepository, IBrandCommandRepository { }
}

namespace Infrastructure.Repositories.Queries
{
    public class BrandQueryRepository : QueryRepository, IBrandQueryRepository { }
}
```

---

## ?? Target Directory Structure

### Application Folder
```
Application/
?? Features/                           ? NEW STANDARD
?  ?? Account/
?  ?  ?? Commands/
?  ?  ?  ?? Create/
?  ?  ?  ?  ?? CreateAccountCommand.cs
?  ?  ?  ?? Update/
?  ?  ?  ?  ?? UpdateAccountCommand.cs
?  ?  ?  ?? Delete/
?  ?  ?     ?? DeleteAccountCommand.cs
?  ?  ?? Queries/
?  ?     ?? GetAll/
?  ?     ?  ?? GetAllAccountsQuery.cs
?  ?     ?? GetById/
?  ?        ?? GetAccountByIdQuery.cs
?  ?? Brand/
?  ?  ?? Commands/
?  ?  ?  ?? Create/CreateBrandCommand.cs
?  ?  ?  ?? Update/UpdateBrandCommand.cs
?  ?  ?  ?? Delete/DeleteBrandCommand.cs
?  ?  ?? Queries/
?  ?     ?? GetAll/GetAllBrandsQuery.cs
?  ?     ?? GetById/GetBrandByIdQuery.cs
?  ?? Auth/
?  ?  ?? Commands/
?  ?     ?? AuthCommand.cs
?  ?? User/
?  ?  ?? Commands/
?  ?  ?? Queries/
?  ?? Role/
?  ?  ?? Commands/
?  ?  ?? Queries/
?  ?? Product/ ... (17 more features)
?
?? DTOs/                              ? CONSOLIDATED HERE
?  ?? Accounting/
?  ?  ?? AccountDto.cs
?  ?  ?? JournalEntryDto.cs
?  ?  ?? JournalEntryLineDto.cs
?  ?? Core/
?  ?  ?? BrandDto.cs
?  ?  ?? BranchDto.cs
?  ?  ?? EmployeeDto.cs
?  ?  ?? CustomerDto.cs
?  ?? Products/
?  ?  ?? ProductDto.cs
?  ?  ?? ProductCategoryDto.cs
?  ?  ?? BatchDto.cs
?  ?  ?? ...
?  ?? Purchasing/
?  ?  ?? SupplierDto.cs
?  ?  ?? PurchaseDto.cs
?  ?  ?? PurchaseItemDto.cs
?  ?? Orders/
?  ?  ?? OrderDto.cs
?  ?  ?? OrderItemDto.cs
?  ?? Expenses/
?  ?  ?? ExpenseDto.cs
?  ?  ?? ExpenseCategoryDto.cs
?  ?? Auth/
?     ?? AuthResponseDTO.cs
?     ?? UserDetailsResponseDTO.cs
?     ?? ...
?
?? Interfaces/
?  ?? IIdentityService.cs
?  ?? ITokenGenerator.cs
?  ?? IUnitOfWork.cs
?  ?? IMediaStorage.cs
?
?? Bases/
?  ?? Response.cs         ? FIXED
?  ?? ResponseHandler.cs
?  ?? BaseHandler.cs
?
?? QueryRepositories/
?  ?? (Query repository interfaces)
?
?? ApplicationRegistration.cs
```

---

## ?? Migration Checklist

### Phase 1: Analysis ? DONE
- [x] Identify namespace issues
- [x] Document current state
- [x] Plan new structure
- [x] Create migration guide

### Phase 2: Core Fixes ? DONE
- [x] Fix `CleanArchitecture.Application.Bases`
- [x] Fix `CleanArchitecture.Api.Middleware`
- [x] Update Program.cs

### Phase 3: Consolidation ? PENDING
- [ ] Create `Application\Features` folder
- [ ] Move 40 commands to `Application.Features.[Entity].Commands`
- [ ] Move 20 queries to `Application.Features.[Entity].Queries`
- [ ] Move all DTOs to `Application.DTOs.[Domain]`
- [ ] Move Auth/User/Role to Features

### Phase 4: Import Updates ? PENDING
- [ ] Update all controller imports
- [ ] Update all handler imports
- [ ] Update all DI registrations
- [ ] Update Program.cs imports

### Phase 5: Validation ? PENDING
- [ ] Verify build (0 errors)
- [ ] Run unit tests
- [ ] Test all endpoints
- [ ] Code review

---

## ?? File Count by Category

| Category | Current | Target Namespace | Files |
|----------|---------|------------------|-------|
| Commands | Mixed | `Application.Features.[Entity].Commands` | 40 |
| Queries | Mixed | `Application.Features.[Entity].Queries` | 20 |
| DTOs | Scattered | `Application.DTOs.[Domain]` | 30 |
| Controllers | API.Controllers | API.Controllers | 20 |
| Repositories | Infrastructure.Repositories | Infrastructure.Repositories | 40 |

**Total Scope**: ~150 files requiring verification

---

## ?? Key Warnings

### Things to Watch
1. **Case Sensitivity**: `DTOs` vs `Dtos` - choose one consistently
2. **Folder Creation**: Ensure folders match namespace
3. **File Moves**: Update project references after moving
4. **Build Validation**: Test frequently during migration
5. **Import Updates**: Use Find & Replace carefully

### Common Mistakes to Avoid
? Changing just namespace without moving file
? Forgetting to update dependencies
? Missing case consistency (DTOs vs Dtos)
? Not updating Program.cs registrations
? Partial migrations (some files updated, others not)

---

## ? Validation Commands

### Build Verification
```bash
dotnet clean
dotnet build
```

### Find Remaining Issues
```bash
# Search for old namespaces
grep -r "using CleanArchitecture" .
grep -r "using Authentication.Application" .
grep -r "using Application.UseCases" .
```

### Run Tests
```bash
dotnet test
```

---

## ?? Related Documentation

1. **NAMESPACE_REORGANIZATION_PLAN.md**
   - Detailed structure plan
   - Directory layout
   - Benefits

2. **NAMESPACE_FIX_IMPLEMENTATION.md**
   - Step-by-step instructions
   - Before/after examples
   - Priority fixes

3. **NAMESPACE_CONSOLIDATION_SUMMARY.md**
   - Current audit results
   - Implementation roadmap
   - Validation checklist

4. **This File (COMPLETE_NAMESPACE_GUIDE.md)**
   - End-to-end overview
   - Standard conventions
   - Migration steps

---

## ?? Success Criteria

After implementation, verify:

? No `CleanArchitecture.*` namespaces
? No `Authentication.Application.*` namespaces
? No `Application.UseCases.*` namespaces
? All commands in `Application.Features.*.Commands`
? All queries in `Application.Features.*.Queries`
? All DTOs in `Application.DTOs.*`
? Consistent casing (all use `DTOs` not `Dtos`)
? All controllers in `API.Controllers`
? All middleware in `API.Middleware`
? Project builds successfully
? All tests pass
? No compilation warnings

---

## ?? Next Steps

1. **Review** this document with team
2. **Approve** the proposed structure
3. **Create** `Application\Features` folder
4. **Start** moving commands and queries
5. **Update** imports incrementally
6. **Test** after each phase
7. **Deploy** after full validation

---

## ?? Pro Tips

### Use VS Code Find & Replace
- Pattern: `using Application.UseCases.Commands.(.*) import`
- Replace with: `using Application.Features.$1.Commands`

### Create Script
```powershell
# Move all commands
Move-Item "Application\UseCases\Commands\*" "Application\Features"
```

### Verify with Grep
```bash
# Check for remaining old namespaces
grep -r "Application.UseCases.Commands" --include="*.cs" .
```

---

## ?? Questions?

Refer to:
- **How to organize?** ? NAMESPACE_REORGANIZATION_PLAN.md
- **What to fix?** ? NAMESPACE_FIX_IMPLEMENTATION.md
- **Current status?** ? NAMESPACE_CONSOLIDATION_SUMMARY.md
- **Step by step?** ? This document

---

## Summary

? **Phase 1 Complete**: Core namespace issues fixed
?? **Phase 2 Ready**: Full consolidation awaiting approval
?? **Impact**: 150 files, ~200+ imports
?? **Estimated Time**: 2-3 hours
? **Benefit**: Logical, maintainable, scalable codebase

**Status**: Ready to proceed to Phase 2

---

**Last Updated**: 2024
**Framework**: .NET 10, C# 14
**Database**: PostgreSQL

?? **Namespace reorganization in progress!**
