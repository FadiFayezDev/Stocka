# ?? NAMESPACE REORGANIZATION PLAN

## Current Issues Found

### ? Problems
1. **Mixed Namespaces**:
   - `Application.UseCases.Commands` vs `Application.Features.Commands`
   - `Authentication.Application.Commands` vs `Application.UseCases.Commands`
   - `CleanArchitecture.Application.Bases` vs `Application.Bases`

2. **Inconsistent Structure**:
   - Some files in `Application.Results` should be in `Application.Bases`
   - Queries in different namespaces
   - DTOs scattered across multiple locations

3. **Missing Organization**:
   - No clear separation between features
   - Middleware in wrong namespace (`CleanArchitecture.Api.Middleware`)
   - Helper classes not properly organized

## ? NEW LOGICAL STRUCTURE

### Domain Layer
```
Domain/
?? Bases/
?  ?? IEntity<T>
?? Entities/
?  ?? Core/          (Brand, Branch, Employee, Customer)
?  ?? Products/      (Product, Batch, Warehouse, etc.)
?  ?? Accounting/    (Account, JournalEntry, etc.)
?  ?? Purchasing/    (Supplier, Purchase, etc.)
?  ?? Orders/        (Order, OrderItem)
?  ?? Expenses/      (Expense, ExpenseCategory)
?? Enums/
?? ValueObjects/
?? Contracts/ (IEntity<T>, IEntity interfaces)
```

### Application Layer
```
Application/
?? Bases/
?  ?? ResponseHandler.cs
?  ?? Response<T>.cs
?? DTOs/
?  ?? Accounting/
?  ?? Core/
?  ?? Products/
?  ?? Purchasing/
?  ?? Orders/
?  ?? Expenses/
?? Interfaces/
?  ?? IIdentityService.cs
?  ?? ITokenGenerator.cs
?  ?? IUnitOfWork.cs
?  ?? IMediaStorage.cs
?? Features/          ? NEW: Organized by feature
?  ?? Brands/
?  ?  ?? Commands/
?  ?  ?  ?? Create/
?  ?  ?  ?? Update/
?  ?  ?  ?? Delete/
?  ?  ?? Queries/
?  ?     ?? GetAll/
?  ?     ?? GetById/
?  ?? Products/
?  ?? Accounts/
?  ?? ... (20 features)
?? QueryRepositories/
?? Repositories/
?? ApplicationRegistration.cs
```

### Infrastructure Layer
```
Infrastructure/
?? Contexts/
?  ?? AppDbContext.cs
?? Identity/
?  ?? ApplicationUser.cs
?  ?? Services/ (IdentityService.cs)
?? Services/
?  ?? TokenGenerator.cs
?  ?? UnitOfWork.cs
?  ?? MediaStorage.cs
?  ?? Helpers/
?? Repositories/
?  ?? Commands/
?  ?  ?? Base/
?  ?  ?? [Entity]CommandRepository
?  ?? Queries/
?     ?? Base/
?     ?? [Entity]QueryRepository
?? Configurations/
?  ?? [Entity]Configuration
?? Migrations/
?? InfrastructureRegistration.cs
```

### API Layer
```
API/
?? Controllers/
?  ?? Base/
?  ?  ?? BaseController.cs
?  ?? Brands/
?  ?  ?? BrandsController.cs
?  ?? Products/
?  ?  ?? ProductsController.cs
?  ?? ... (organized by feature)
?? Middleware/
?  ?? GlobalExceptionMiddleware.cs
?? Program.cs
?? appsettings.json
?? Dockerfile
```

---

## Implementation Steps

### Phase 1: Application Layer Reorganization
1. Create new `Features` folder structure
2. Move all Commands to `Application.Features.[Entity].Commands`
3. Move all Queries to `Application.Features.[Entity].Queries`
4. Update all namespace declarations

### Phase 2: Fix Core Namespaces
1. Consolidate `Application.Bases` and `Application.Results`
2. Move all DTOs to `Application.DTOs`
3. Fix `Authentication.Application` references

### Phase 3: API Layer
1. Organize controllers by feature
2. Fix `CleanArchitecture.Api.Middleware` ? `API.Middleware`
3. Ensure all imports use correct namespaces

### Phase 4: Update All References
1. Fix all using statements
2. Update dependency injection
3. Update imports in Program.cs

---

## File Migration Map

### Commands
- `Application.UseCases.Commands.Brand` ? `Application.Features.Brand.Commands`
- `Application.UseCases.Commands.Product` ? `Application.Features.Product.Commands`
- ... (40 commands total)

### Queries
- `Authentication.Application.Queries.User` ? `Application.Features.User.Queries`
- `Application.Queries.Account` ? `Application.Features.Account.Queries`
- ... (20 queries total)

### DTOs
- `Application.Dtos` ? Keep as is, organize by domain

### Middleware
- `CleanArchitecture.Api.Middleware` ? `API.Middleware`

---

## Namespace Convention

### Final Standard Format
```
[Project].[Layer].[Feature].[SubFeature].[Component]

Examples:
- Domain.Entities.Core.Brand
- Application.Features.Brand.Commands.Create
- Application.Features.Brand.Queries.GetAll
- Application.DTOs.Core.BrandDto
- Infrastructure.Repositories.Commands.BrandCommandRepository
- Infrastructure.Repositories.Queries.BrandQueryRepository
- API.Controllers.Brands.BrandsController
```

---

## Benefits
? Consistent naming convention
? Clear feature-based organization
? Easy to navigate codebase
? Logical separation of concerns
? Future-proof structure
