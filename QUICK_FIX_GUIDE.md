# Quick Fix Guide - 6 Pending Controllers

This document provides the exact fixes needed for the 6 controllers that have namespace import issues.

## Issue Summary

6 controllers have import errors due to inconsistent namespace naming in the codebase:
- Some commands/queries use `Application.UseCases`
- Some commands/queries use `Application.Features`

## Fix #1: BranchesController

**File**: `API\Controllers\BranchesController.cs`

**Change these imports**:
```csharp
// OLD - REMOVE
using Application.UseCases.Commands.Branch.Create;
using Application.UseCases.Commands.Branch.Delete;
using Application.UseCases.Commands.Branch.Update;
using Application.Queries.Branch.GetById;
using Application.Queries.Branch.GetAll;

// NEW - ADD
using Application.Features.Commands.Branch.Create;
using Application.Features.Commands.Branch.Delete;
using Application.Features.Commands.Branch.Update;
using Application.Features.Queries.Branch.GetById;
using Application.Features.Queries.Branch.GetAll;
```

---

## Fix #2: PurchasesController

**File**: `API\Controllers\PurchasesController.cs`

**Change these imports**:
```csharp
// OLD - REMOVE
using Application.UseCases.Commands.Purchase.Create;
using Application.UseCases.Commands.Purchase.Delete;
using Application.UseCases.Commands.Purchase.Update;
using Application.Queries.Purchase.GetById;
using Application.Queries.Purchase.GetAll;

// NEW - ADD
using Application.Features.Commands.Purchase.Create;
using Application.Features.Commands.Purchase.Delete;
using Application.Features.Commands.Purchase.Update;
using Application.Features.Queries.Purchase.GetById;
using Application.Features.Queries.Purchase.GetAll;
```

---

## Fix #3: WarehouseBatchesController

**File**: `API\Controllers\WarehouseBatchesController.cs`

**Change these imports**:
```csharp
// OLD - REMOVE
using Application.UseCases.Commands.WarehouseBatch.Create;
using Application.UseCases.Commands.WarehouseBatch.Delete;
using Application.UseCases.Commands.WarehouseBatch.Update;
using Application.Queries.WarehouseBatch.GetById;
using Application.Queries.WarehouseBatch.GetAll;

// NEW - ADD
using Application.Features.Commands.WarehouseBatch.Create;
using Application.Features.Commands.WarehouseBatch.Delete;
using Application.Features.Commands.WarehouseBatch.Update;
using Application.Features.Queries.WarehouseBatch.GetById;
using Application.Features.Queries.WarehouseBatch.GetAll;
```

---

## Fix #4: ProductCategoriesController

**File**: `API\Controllers\ProductCategoriesController.cs`

**Change these imports**:
```csharp
// OLD - REMOVE
using Application.UseCases.Commands.ProductCategory.Create;
using Application.UseCases.Commands.ProductCategory.Delete;
using Application.UseCases.Commands.ProductCategory.Update;
using Application.Queries.ProductCategory.GetById;
using Application.Queries.ProductCategory.GetAll;

// NEW - ADD
using Application.Features.Commands.ProductCategory.Create;
using Application.Features.Commands.ProductCategory.Delete;
using Application.Features.Commands.ProductCategory.Update;
using Application.Features.Queries.ProductCategory.GetById;
using Application.Features.Queries.ProductCategory.GetAll;
```

---

## Fix #5: ExpenseCategoriesController

**File**: `API\Controllers\ExpenseCategoriesController.cs`

**Change these imports**:
```csharp
// OLD - REMOVE
using Application.UseCases.Commands.ExpenseCategory.Create;
using Application.UseCases.Commands.ExpenseCategory.Delete;
using Application.UseCases.Commands.ExpenseCategory.Update;
using Application.Queries.ExpenseCategory.GetById;
using Application.Queries.ExpenseCategory.GetAll;

// NEW - ADD
using Application.Features.Commands.ExpenseCategory.Create;
using Application.Features.Commands.ExpenseCategory.Delete;
using Application.Features.Commands.ExpenseCategory.Update;
using Application.Features.Queries.ExpenseCategory.GetById;
using Application.Features.Queries.ExpenseCategory.GetAll;
```

---

## Fix #6: SuppliersController

**File**: `API\Controllers\SuppliersController.cs`

**Status**: Partially Fixed (Commands use Features, Queries use UseCases)

**Change these imports**:
```csharp
// CURRENT - ALREADY CORRECT
using Application.Features.Commands.Supplier.Create;
using Application.Features.Commands.Supplier.Delete;
using Application.Features.Commands.Supplier.Update;

// These need to be checked - use Features if available
using Application.Features.Queries.Supplier.GetById;  // Try this first
using Application.Features.Queries.Supplier.GetAll;   // Try this first

// If Features doesn't work, use:
// using Application.Queries.Supplier.GetById;
// using Application.Queries.Supplier.GetAll;
```

---

## Fix #7: StockMovementsController

**File**: `API\Controllers\StockMovementsController.cs`

**Change these imports**:
```csharp
// OLD - REMOVE (if present)
// using Application.UseCases.Commands.StockMovement.Create;

// CURRENT - ALREADY CORRECT
using Application.Features.Commands.StockMovement.Create;
using Application.Features.Commands.StockMovement.Delete;
using Application.Features.Commands.StockMovement.Update;

// These queries need Features namespace
// CHANGE FROM:
// using Application.Queries.StockMovement.GetById;
// using Application.Queries.StockMovement.GetAll;

// TO:
// using Application.Features.Queries.StockMovement.GetById;
// using Application.Features.Queries.StockMovement.GetAll;

// OR (if Features.Queries doesn't exist):
// using Application.Queries.StockMovement.GetById;
// using Application.Queries.StockMovement.GetAll;
```

---

## How to Apply Fixes

### Method 1: Manual Edit
1. Open each problematic controller file
2. Replace the import statements as shown above
3. Save the file
4. Run `dotnet build`

### Method 2: Find & Replace (in Visual Studio)
1. Open Find & Replace (Ctrl+H)
2. For each fix, replace the old import with new import
3. Save all files
4. Run `dotnet build`

### Method 3: Using Script
Create a PowerShell script to apply fixes automatically (if comfortable with scripting).

---

## Verification

After applying all fixes, run:
```bash
dotnet build
```

Expected output:
```
Build succeeded with 0 errors.
```

---

## Testing

After build succeeds:

```bash
# Run unit tests
dotnet test

# Run API
dotnet run --project API

# Navigate to
http://localhost:5000/swagger/ui
```

---

## Summary

- **Time to fix**: ~15 minutes
- **Changes needed**: Import statement updates only
- **Logic changes**: None
- **Risk level**: Low (no functional changes)

Once these imports are fixed, all 20 controllers will:
- ? Compile successfully
- ? Work with MediatR
- ? Connect to handlers
- ? Be ready for testing

---

## Support

If namespace issues persist after fixes:

1. **Verify file locations** - Check if the command/query actually exists at the expected path
2. **Check naming** - Ensure class names match (e.g., `CreateBranchCommand`)
3. **Check namespace declarations** - Open the actual command/query file and verify its namespace
4. **Update based on actual namespace** - Use whatever namespace is declared in the file

Good luck! ??
