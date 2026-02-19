# Complete Handler Implementation Guide

## Overview
This document describes the comprehensive implementation of proper handler logic for all CRUD operations (Create, Update, Delete) across all entities in the Application layer.

## Architecture Pattern

### BaseHandler Enhancement
The `BaseHandler<T>` and `BaseHandler<Command, Query>` classes have been enhanced with three protected helper methods:

```csharp
protected async Task<Response<TDto>> ExecuteCreateAsync<TEntity, TDto>(...)
protected async Task<Response<TDto>> ExecuteUpdateAsync<TEntity, TDto>(...)
protected async Task<Response<bool>> ExecuteDeleteAsync<TEntity>(...)
```

These methods handle:
- **Transaction Management**: Begin ? Execute ? Commit/Rollback
- **Error Handling**: Automatic rollback on failure
- **Validation**: Checking operation results
- **DTO Mapping**: Converting entities back to DTOs

### Key Features Implemented

1. **Transaction Support**
   - All operations wrapped in transactions
   - Automatic rollback on exceptions
   - Explicit transaction commit after successful persistence

2. **Unified Error Handling**
   - Consistent error messages across all handlers
   - Proper HTTP status codes via Response<T>
   - Transaction rollback on errors

3. **UnitOfWork Integration**
   - All handlers inject IUnitOfWork
   - SaveChangesAsync called with cancellation token support
   - Transaction lifecycle management

## Implemented Handlers

### 1. Account Commands
- **CreateAccountCommand**: Maps DTO ? Entity ? Create ? Map back
- **UpdateAccountCommand**: Get entity from command repo ? Map updates ? Update
- **DeleteAccountCommand**: Get entity ? Delete with transaction

### 2. Brand Commands
- **CreateBrandCommand**: Standard create pattern with transaction
- **UpdateBrandCommand**: Get existing ? Map changes ? Update
- **DeleteBrandCommand**: Validates existence ? Delete

### 3. Branch Commands
- **CreateBranchCommand**: Create with transaction support
- **UpdateBranchCommand**: Standard update pattern
- **DeleteBranchCommand**: Delete with validation

### 4. Employee Commands
- **CreateEmployeeCommand**: Uses domain entity constructor
- **UpdateEmployeeCommand**: Uses domain entity methods (UpdateJobTitle, UpdateSalary, Activate/Deactivate)
- **DeleteEmployeeCommand**: Standard delete pattern

### 5. Customer Commands
- **CreateCustomerCommand**: Direct entity creation
- **UpdateCustomerCommand**: Gets entity from command repo, applies changes
- **DeleteCustomerCommand**: Delete with validation

### 6. Warehouse Commands
- **CreateWarehouseCommand**: Standard create with UnitOfWork
- **UpdateWarehouseCommand**: Update with mapper
- **DeleteWarehouseCommand**: Delete with transaction

### 7. Product Commands
- **CreateProductCommand**: Create with proper transaction handling
- **UpdateProductCommand**: Update existing product
- **DeleteProductCommand**: Delete with validation

### 8. ProductCategory Commands
- **CreateProductCategoryCommand**: Create with transaction
- **UpdateProductCategoryCommand**: Update category
- **DeleteProductCategoryCommand**: Delete category

### 9. Supplier Commands
- **CreateSupplierCommand**: Create supplier entity
- **UpdateSupplierCommand**: Update supplier details
- **DeleteSupplierCommand**: Delete supplier

### 10. Purchase Commands
- **CreatePurchaseCommand**: Create purchase
- **UpdatePurchaseCommand**: Update purchase
- **DeletePurchaseCommand**: Delete purchase

### 11. PurchaseItem Commands
- **CreatePurchaseItemCommand**: Create purchase item
- **UpdatePurchaseItemCommand**: Update purchase item
- **DeletePurchaseItemCommand**: Delete purchase item

### 12. Batch Commands
- **CreateBatchCommand**: Create with timestamp
- **UpdateBatchCommand**: Update batch details
- **DeleteBatchCommand**: Delete batch

### 13. WarehouseBatch Commands
- **CreateWarehouseBatchCommand**: Create warehouse batch
- **UpdateWarehouseBatchCommand**: Update warehouse batch
- **DeleteWarehouseBatchCommand**: Delete warehouse batch

### 14. Expense Commands
- **CreateExpenseCommand**: Create expense
- **UpdateExpenseCommand**: Update expense
- **DeleteExpenseCommand**: Delete expense

### 15. ExpenseCategory Commands
- **CreateExpenseCategoryCommand**: Create category
- **UpdateExpenseCategoryCommand**: Update category
- **DeleteExpenseCategoryCommand**: Delete category

### 16. StockMovement Commands
- **CreateStockMovementCommand**: Create with timestamp
- **UpdateStockMovementCommand**: Update movement
- **DeleteStockMovementCommand**: Delete movement

### 17. Sale/Order Commands
- **CreateOrderCommand**: Create order
- **UpdateOrderCommand**: Update order
- **DeleteOrderCommand**: Delete order

### 18. SaleItem/OrderItem Commands
- **CreateOrderItemCommand**: Create order item
- **UpdateOrderItemCommand**: Update order item
- **DeleteOrderItemCommand**: Delete order item

### 19. JournalEntry Commands
- **CreateJournalEntryCommand**: Create journal entry
- **UpdateJournalEntryCommand**: Update journal entry
- **DeleteJournalEntryCommand**: Delete journal entry

### 20. JournalEntryLine Commands
- **CreateJournalEntryLineCommand**: Create line item
- **UpdateJournalEntryLineCommand**: Update line item
- **DeleteJournalEntryLineCommand**: Delete line item

## Implementation Pattern

### Create Handler Pattern
```csharp
public class CreateXyzCommandHandler : BaseHandler<IXyzCommandRepository>, 
    IRequestHandler<CreateXyzCommand, Response<XyzDto>>
{
    public async Task<Response<XyzDto>> Handle(CreateXyzCommand request, CancellationToken ct)
    {
        var entity = _mapper.Map<XyzEntity>(request);
        
        return await ExecuteCreateAsync<XyzEntity, XyzDto>(
            entity,
            async (e) => await _repo.CreateAsync(e),
            ct);
    }
}
```

### Update Handler Pattern
```csharp
public class UpdateXyzCommandHandler : BaseHandler<IXyzCommandRepository>, 
    IRequestHandler<UpdateXyzCommand, Response<XyzDto>>
{
    public async Task<Response<XyzDto>> Handle(UpdateXyzCommand request, CancellationToken ct)
    {
        var existing = await _repo.GetByIdAsync(request.Id);
        if (existing == null)
            return new Response<XyzDto>(false, "Not found");

        _mapper.Map(request, existing);
        
        return await ExecuteUpdateAsync<XyzEntity, XyzDto>(
            existing,
            async (e) => await _repo.UpdateAsync(e),
            ct);
    }
}
```

### Delete Handler Pattern
```csharp
public class DeleteXyzCommandHandler : BaseHandler<IXyzCommandRepository>, 
    IRequestHandler<DeleteXyzCommand, Response<bool>>
{
    public async Task<Response<bool>> Handle(DeleteXyzCommand request, CancellationToken ct)
    {
        var existing = await _repo.GetByIdAsync(request.Id);
        if (existing == null)
            return new Response<bool>(false, "Not found");

        return await ExecuteDeleteAsync(
            existing,
            async (e) => await _repo.DeleteAsync(e),
            ct);
    }
}
```

## Important Considerations

### Query Repository vs Command Repository
- **Query Repositories** return DTOs (Dapper-based)
- **Command Repositories** return Entities (EF Core-based)
- For Update/Delete: Use **Command Repository** to get entities
- For Read-only: Use **Query Repository** for DTO queries

### Domain Entity Methods
Some entities have protected setters and require using domain methods:
- **Employee**: Use `UpdateJobTitle()`, `UpdateSalary()`, `Activate()`, `Deactivate()`
- **Expense**: Use `UpdateAmount()`, `UpdateNotes()`, `UpdateExpenseDate()`
- Other entities: Use public properties directly

### Transaction Handling
All database operations are wrapped in:
1. `BeginTransactionAsync()` - Start transaction
2. Execute operation
3. `SaveChangesAsync()` - Persist changes
4. `CommitTransactionAsync()` - Commit transaction
5. On exception: `RollbackTransactionAsync()` - Rollback all changes

## Response Format
All handlers return `Response<T>` with:
- `Succeeded`: Boolean indicating success
- `Data`: The DTO result (for successful operations)
- `Message`: User-friendly message
- `StatusCode`: HTTP status code

## Error Handling Strategy
- **Not Found**: Return `Response<T>(false, "Entity not found")`
- **Operation Failure**: Automatic transaction rollback
- **Exceptions**: Logged, transaction rolled back, exception re-thrown

## Benefits of This Implementation

1. **DRY Principle**: Shared logic in ExecuteAsync methods
2. **Consistency**: Same pattern across all 60+ handlers
3. **Reliability**: Automatic transaction management
4. **Maintainability**: Changes to transaction logic in one place
5. **Error Safety**: Automatic rollback on exceptions
6. **Testability**: Clear separation of concerns

## Testing Recommendations

1. **Happy Path**: Entity created/updated/deleted successfully
2. **Not Found**: Return proper error response
3. **Validation**: Invalid data returns appropriate error
4. **Transactions**: Verify rollback on exception
5. **Concurrency**: Test cancellation token handling
