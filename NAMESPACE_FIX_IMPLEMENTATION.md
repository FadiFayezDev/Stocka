# ?? NAMESPACE FIXES - IMPLEMENTATION GUIDE

## Summary of Issues Fixed

### ? Fixed
1. **Application/Results/Response.cs**
   - Was: `CleanArchitecture.Application.Bases`
   - Now: `Application.Bases`

2. **API/Program.cs**
   - Was: `using CleanArchitecture.Api.Middleware`
   - Now: `using API.Middleware`

---

## Remaining Critical Fixes Needed

### 1. Authentication/Application Commands
These should be moved under Application.Features

```
Current Structure:
Authentication.Application.Commands.Auth
Authentication.Application.Commands.User
Authentication.Application.DTOs
Authentication.Application.Queries.Role
Authentication.Application.Queries.User

Should Be:
Application.Features.Auth.Commands
Application.Features.User.Commands
Application.Features.User.Queries
Application.Features.Role.Queries
```

---

### 2. Mixed Command Namespaces

#### Current Issues:
```csharp
// Some use:
using Application.UseCases.Commands.Brand.Create;

// Some use:
using Authentication.Application.Commands.Auth;

// Some use:
using Application.UseCases.Commands.User.Create;
```

#### Should All Use:
```csharp
using Application.Features.Brand.Commands.Create;
using Application.Features.Auth.Commands;
using Application.Features.User.Commands.Create;
```

---

### 3. Query Namespaces Consolidation

#### Current Issues:
```csharp
using Application.Queries.Account.GetAll;
using Authentication.Application.Queries.User;
using Application.Queries.Brand.GetAll;
```

#### Should All Use:
```csharp
using Application.Features.Account.Queries.GetAll;
using Application.Features.User.Queries;
using Application.Features.Brand.Queries.GetAll;
```

---

## File-by-File Migration Map

### Controllers (API Layer)
All controller namespaces should use:
```csharp
namespace API.Controllers
```

**Current Status**: ? Already correct in most controllers

---

### Repositories (Infrastructure Layer)

#### Command Repositories
```csharp
// Correct format:
namespace Infrastructure.Repositories.Commands
{
    public class BrandCommandRepository : CommandRepository, IBrandCommandRepository
}
```

#### Query Repositories
```csharp
// Correct format:
namespace Infrastructure.Repositories.Queries
{
    public class JournalEntryQueryRepository : QueryRepository, IJournalEntryQueryRepository
}
```

---

### DTOs (Application Layer)

```csharp
// Organize by domain:
namespace Application.DTOs.Accounting
namespace Application.DTOs.Core
namespace Application.DTOs.Products
namespace Application.DTOs.Purchasing
namespace Application.DTOs.Orders
namespace Application.DTOs.Expenses
namespace Application.DTOs.Auth
```

---

## Priority Order for Fixes

### ?? Critical (Phase 1)
1. Fix `CleanArchitecture.Api.Middleware` ? `API.Middleware`
2. Fix `CleanArchitecture.Application.Bases` ? `Application.Bases`
3. Consolidate `Authentication.Application` ? `Application.Features`

### ?? High Priority (Phase 2)
4. Standardize all Commands to `Application.Features.[Entity].Commands`
5. Standardize all Queries to `Application.Features.[Entity].Queries`
6. Consolidate all DTOs to `Application.DTOs.[Domain]`

### ?? Medium Priority (Phase 3)
7. Organize Controllers by feature domain
8. Update repository interfaces to correct namespaces
9. Fix configuration classes namespaces

---

## Files Requiring Updates

### Controllers Needing Namespace Fixes

Files using `Authentication.Application` imports:
- `API\Controllers\AuthController.cs`
- `API\Controllers\UserController.cs`
- `API\Controllers\RoleController.cs`

These should import from `Application.Features` instead.

---

## Consolidation Examples

### Example 1: User Commands

```csharp
// BEFORE:
using Authentication.Application.Commands.User.Create;
using Application.UseCases.Commands.User.Create;

// AFTER:
using Application.Features.User.Commands.Create;
```

### Example 2: Brand Queries

```csharp
// BEFORE:
using Application.Queries.Brand.GetAll;

// AFTER:
using Application.Features.Brand.Queries.GetAll;
```

### Example 3: Account Commands

```csharp
// BEFORE:
using Application.UseCases.Commands.Account.Create;

// AFTER:
using Application.Features.Account.Commands.Create;
```

---

## New Standard Structure

```
Application/
?? Features/
?  ?? Account/
?  ?  ?? Commands/
?  ?  ?  ?? Create/CreateAccountCommand.cs
?  ?  ?  ?? Update/UpdateAccountCommand.cs
?  ?  ?  ?? Delete/DeleteAccountCommand.cs
?  ?  ?? Queries/
?  ?     ?? GetAll/GetAllAccountsQuery.cs
?  ?     ?? GetById/GetAccountByIdQuery.cs
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
?  ?  ?  ?? AuthCommand.cs
?  ?  ?? Queries/
?  ?? ... (18 more features)
?? DTOs/
?  ?? Accounting/AccountDto.cs
?  ?? Core/BrandDto.cs
?  ?? Auth/AuthResponseDTO.cs
?  ?? ...
?? Interfaces/
?? Bases/
?? QueryRepositories/
```

---

## Benefits of This Organization

? **Consistency**: All commands, queries, and DTOs follow same pattern
? **Discoverability**: Easy to find related code for a feature
? **Scalability**: New features follow same structure
? **Maintainability**: Clear boundaries between features
? **Testing**: Feature-based testing becomes easier
? **Documentation**: Self-documenting structure

---

## Next Steps

1. Review this document
2. Create new `Application\Features` folder structure
3. Move commands and queries to new structure
4. Update all namespace declarations
5. Update all import statements
6. Run build to verify no compilation errors
7. Test all endpoints to ensure functionality

---

## Validation Checklist

- [ ] No `Authentication.Application` namespaces remain
- [ ] No `CleanArchitecture` namespaces remain
- [ ] No `Application.UseCases` namespaces remain (migrate to `Application.Features`)
- [ ] All Commands use `Application.Features.[Entity].Commands`
- [ ] All Queries use `Application.Features.[Entity].Queries`
- [ ] All DTOs use `Application.DTOs.[Domain]`
- [ ] All Controllers use `API.Controllers`
- [ ] Middleware uses `API.Middleware`
- [ ] All imports compile without errors
- [ ] All tests pass

---

## Files Already Fixed ?

1. `Application\Results\Response.cs` - Namespace corrected
2. `API\Program.cs` - Middleware import corrected

## Files Still Need Fixing ?

See next document for detailed migration instructions.
