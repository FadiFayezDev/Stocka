# ???? ????: ??????? ??? Updated Configurations

## ?? ????? ????????? ?? ??? New Configurations

### 1. ????? Customer ?? Default LoyaltyPoints

```csharp
// Application/UseCases/Commands/Customer/Create/CreateCustomerCommand.cs

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Response<CustomerDto>>
{
    private readonly ICustomerCommandRepository _repository;
    private readonly IMapper _mapper;

    public async Task<Response<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        // ? ??????? ??? Constructor ??????
        var customer = new Customer(
            userId: request.ApplicationUserId,
            brandId: request.BrandId,
            initialLoyaltyPoints: 0  // ?? ???? ????
        );

        await _repository.CreateAsync(customer);
        var customerDto = _mapper.Map<CustomerDto>(customer);
        
        return new Response<CustomerDto>(customerDto, "Created Successfully");
    }
}
```

**Database Result:**
```sql
INSERT INTO Customers (Id, UserId, BrandId, LoyaltyPoints)
VALUES (NEWID(), @UserId, @BrandId, 0);
-- ? LoyaltyPoints = 0 ???? ?????? ?? Configuration
```

---

### 2. ????? Purchase ?? Auto-calculated Total

```csharp
// Application/UseCases/Commands/Purchase/Create/CreatePurchaseCommand.cs

public class CreatePurchaseCommandHandler : IRequestHandler<CreatePurchaseCommand, Response<PurchaseDto>>
{
    private readonly IPurchaseCommandRepository _repository;
    private readonly IPurchaseItemRepository _itemRepository;

    public async Task<Response<PurchaseDto>> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
    {
        // ? ??????? ??? Constructor ??????
        var purchase = new Purchase(
            brandId: request.BrandId,
            supplierId: request.SupplierId
            // PurchaseDate ????? ???? ???? ?????? (GETUTCDATE())
            // TotalAmount ????? 0 ???? ??????
        );

        // ? ????? Items ?? ??? Business Logic
        foreach (var itemDto in request.Items)
        {
            var purchaseItem = new PurchaseItem(
                purchaseId: purchase.Id,
                productId: itemDto.ProductId,
                quantity: itemDto.Quantity,
                unitCost: itemDto.UnitCost
            );
            
            // ? ????? TotalAmount ????????
            purchase.AddPurchaseItem(purchaseItem);
        }

        await _repository.CreateAsync(purchase);
        var purchaseDto = _mapper.Map<PurchaseDto>(purchase);
        
        return new Response<PurchaseDto>(purchaseDto, "Created Successfully");
    }
}
```

**Database Result:**
```sql
INSERT INTO Purchases (Id, BrandId, SupplierId, PurchaseDate, TotalAmount)
VALUES (NEWID(), @BrandId, @SupplierId, GETUTCDATE(), 1500.00);
-- ? PurchaseDate ? TotalAmount ?????? ???? ?????? ?? Configuration
```

---

### 3. ????? Sale ?? Auto-included Items

```csharp
// Application/Queries/Sale/GetById/GetSaleByIdQuery.cs

public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, Response<SaleDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public async Task<Response<SaleDto>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        // ? SaleItems ????? ???????? ?? AutoInclude()
        var sale = await _context.Sales
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (sale == null)
            return new Response<SaleDto>("Not found");

        // ? ?????? ??? Items ???? query ??????
        var saleDto = _mapper.Map<SaleDto>(sale);
        saleDto.Items = _mapper.Map<List<SaleItemDto>>(sale.SaleItems);  // ? ?? ??????? ??????
        
        return new Response<SaleDto>(saleDto, "Success");
    }
}
```

**Performance Benefit:**
```
? OLD: 2 Queries (Sale + SaleItems)
? NEW: 1 Query (Sale + SaleItems ?? AutoInclude)
```

---

### 4. ????? Customer Loyalty Points

```csharp
// Application/UseCases/Commands/Customer/UpdateLoyaltyPoints/UpdateLoyaltyPointsCommand.cs

public class UpdateLoyaltyPointsCommandHandler : IRequestHandler<UpdateLoyaltyPointsCommand, Response<CustomerDto>>
{
    private readonly ICustomerCommandRepository _repository;

    public async Task<Response<CustomerDto>> Handle(UpdateLoyaltyPointsCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.CustomerId);
        
        if (customer == null)
            return new Response<CustomerDto>("Not found");

        // ? ??????? ??? Business Methods
        try
        {
            switch (request.Operation)
            {
                case "ADD":
                    customer.AddLoyaltyPoints(request.Points);  // ? Validates > 0
                    break;
                case "DEDUCT":
                    customer.DeductLoyaltyPoints(request.Points);  // ? Validates sufficient balance
                    break;
            }
        }
        catch (ArgumentException ex)
        {
            return new Response<CustomerDto>(false, ex.Message);
        }

        await _repository.UpdateAsync(customer);
        var customerDto = _mapper.Map<CustomerDto>(customer);
        
        return new Response<CustomerDto>(customerDto, "Updated Successfully");
    }
}
```

**Validation Benefits:**
```
? InvalidOperationException ??? ???? ??? Points ??? ?????
? ArgumentException ??? ???? ??? Points <= 0
? Logic ????? ?? Entity ????? ?? Application
```

---

### 5. ????? Journal Entry ?? Auto-validation

```csharp
// Application/UseCases/Commands/Accounting/CreateJournalEntry/CreateJournalEntryCommand.cs

public class CreateJournalEntryCommandHandler : IRequestHandler<CreateJournalEntryCommand, Response<JournalEntryDto>>
{
    private readonly IJournalEntryRepository _repository;

    public async Task<Response<JournalEntryDto>> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
    {
        // ? ??????? ??? Constructor
        var entry = new JournalEntry(
            brandId: request.BrandId,
            entryDate: request.EntryDate,
            description: request.Description
        );

        // ? ????? Lines ?? ??? Validation
        try
        {
            foreach (var lineDto in request.Lines)
            {
                var line = new JournalEntryLine(
                    journalEntryId: entry.Id,
                    accountId: lineDto.AccountId,
                    brandId: request.BrandId,
                    debit: lineDto.Debit,
                    credit: lineDto.Credit
                    // ? ???? ArgumentException ??? ???? ??? ???????? > 0
                );
                
                // ? ???? InvalidOperationException ??? ?? ??? ??? Entry ????????
                entry.AddJournalEntryLine(line);
            }

            // ? ???? ?? ???????
            if (!entry.IsBalanced)
                return new Response<JournalEntryDto>(false, "Journal entry is not balanced");

            await _repository.CreateAsync(entry);
            var entryDto = _mapper.Map<JournalEntryDto>(entry);
            
            return new Response<JournalEntryDto>(entryDto, "Created Successfully");
        }
        catch (ArgumentException ex)
        {
            return new Response<JournalEntryDto>(false, $"Invalid line: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            return new Response<JournalEntryDto>(false, $"Invalid entry: {ex.Message}");
        }
    }
}
```

**Auto-Validation Benefits:**
```
? ?? ???? ????? entry ?? Both Debit ? Credit
? ?? ???? ????? entry ???? balance
? ?? Line ???? ?? ??? Entity ????
```

---

### 6. Batch Management ?? Quantity Tracking

```csharp
// Application/UseCases/Commands/Inventory/DeductBatchQuantity/DeductBatchQuantityCommand.cs

public class DeductBatchQuantityCommandHandler : IRequestHandler<DeductBatchQuantityCommand, Response<string>>
{
    private readonly IBatchRepository _repository;

    public async Task<Response<string>> Handle(DeductBatchQuantityCommand request, CancellationToken cancellationToken)
    {
        var batch = await _repository.GetByIdAsync(request.BatchId);
        
        if (batch == null)
            return new Response<string>("Batch not found");

        // ? ??????? ??? Business Logic
        try
        {
            batch.DeductQuantity(request.QuantityToSell);
            
            // ? ???? ??? ???? ??? Batch ???????
            if (batch.IsExhausted)
            {
                // ???? ????? ?????? ?? ?????
                return new Response<string>("Batch exhausted after this sale");
            }

            await _repository.UpdateAsync(batch);
            return new Response<string>($"Remaining: {batch.RemainingQuantity}");
        }
        catch (InvalidOperationException ex)
        {
            return new Response<string>(false, $"Cannot deduct: {ex.Message}");
        }
    }
}
```

**Benefits:**
```
? InvalidOperationException ??? ????? ??? ???? ?? ??????
? IsExhausted property ???? ??????? ?? ??? Edge cases
? Logic ???? ???? Entity ????? ?? Application
```

---

### 7. Sale Status Management

```csharp
// Application/UseCases/Commands/Sales/CompleteSale/CompleteSaleCommand.cs

public class CompleteSaleCommandHandler : IRequestHandler<CompleteSaleCommand, Response<SaleDto>>
{
    private readonly ISaleRepository _repository;

    public async Task<Response<SaleDto>> Handle(CompleteSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.SaleId);
        
        if (sale == null)
            return new Response<SaleDto>("Sale not found");

        // ? ??????? ??? Business Logic ?? ??? Validation
        try
        {
            // ? ?????? ?? ?????? ? ????? ?????? ?? ??? Items
            sale.CompleteSale();
            
            await _repository.UpdateAsync(sale);
            var saleDto = _mapper.Map<SaleDto>(sale);
            
            return new Response<SaleDto>(saleDto, "Sale completed successfully");
        }
        catch (InvalidOperationException ex)
        {
            return new Response<SaleDto>(false, ex.Message);
        }
    }
}
```

**Error Messages:**
```
? "Only pending sales can be completed"
? "Cannot complete a sale without items"
? Entity ???? ??? ??? Business Rules ????????
```

---

### 8. Product Category Management

```csharp
// Application/UseCases/Commands/Product/UpdateProductCategory/UpdateProductCategoryCommand.cs

public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, Response<ProductCategoryDto>>
{
    private readonly IProductCategoryRepository _repository;

    public async Task<Response<ProductCategoryDto>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.CategoryId);
        
        if (category == null)
            return new Response<ProductCategoryDto>("Category not found");

        // ? ??????? ??? Update Method ?? ??? Validation
        try
        {
            category.UpdateName(request.NewName);  // ? Validates empty string
            
            await _repository.UpdateAsync(category);
            var categoryDto = _mapper.Map<ProductCategoryDto>(category);
            
            return new Response<ProductCategoryDto>(categoryDto, "Updated Successfully");
        }
        catch (ArgumentException ex)
        {
            return new Response<ProductCategoryDto>(false, ex.Message);
        }
    }
}
```

---

## ?? ???????

### ??? ?????????:
```
? Collections ???? ?????
? No default values ?? Entity
? No validation ?? Constructor
? Extra queries ???? ??? AutoInclude
? Logic ???? ??? Application ? Domain
```

### ??? ?????????:
```
? Collections ????? ???? ????
? Default values ??????? ?? Database
? Validation ???? ?? Constructor
? Auto-include ???? Queries
? Logic ???? ?? Domain Entity
```

---

## ?? Performance Impact

```csharp
// ? OLD: N+1 Query Problem
var customers = dbContext.Customers.ToList();
foreach (var customer in customers)
{
    var sales = customer.Sales;  // ? ????? query ??? customer
}
// Result: 1 + N queries

// ? NEW: One Query with AutoInclude
var customers = dbContext.Customers.ToList();
foreach (var customer in customers)
{
    var sales = customer.Sales;  // ? ???? query ?????
}
// Result: 1 query ???
```

---

## ? Next Steps

1. ??? ??? ??????? ?? ??? Updated Configurations
2. ???? ?? ?? ??? Business Logic ???? ??? ?? ?????
3. ???? Unit Tests ?? Business Methods
4. ???? ??????? ??? New Constructors ? Methods ?? Application Layer

---

## ?? ????? ??? ???

- `CONFIGURATIONS_UPDATES.md` - ???????? ??????? ?????????
- `MIGRATION_STEPS.md` - ????? ??????? ??? Database
- `Domain/DOMAIN_LAYER_IMPROVEMENTS.md` - ??? ??? Domain Models ???????
- `Domain/USAGE_EXAMPLES.md` - ????? ??????? ??? Domain Models
