# Brand Filtering - Quick Reference Guide

## All New Brand-Filtered Endpoints

### Core Pattern
All brand-filtered endpoints follow this pattern:
```
GET /[controller]/by-brand/{brandId:guid}
```

---

## Complete Endpoint List

| Controller | Endpoint | Returns |
|-----------|----------|---------|
| Products | `GET /products/by-brand/{brandId}` | Products for Brand |
| ProductCategories | `GET /productcategories/by-brand/{brandId}` | Categories for Brand |
| Branches | `GET /branches/by-brand/{brandId}` | Branches for Brand |
| Employees | `GET /employees/by-brand/{brandId}` | Employees for Brand |
| Customers | `GET /customers/by-brand/{brandId}` | Customers for Brand |
| ExpenseCategories | `GET /expensecategories/by-brand/{brandId}` | Categories for Brand |
| Expenses | `GET /expenses/by-brand/{brandId}` | Expenses for Brand |
| Suppliers | `GET /suppliers/by-brand/{brandId}` | Suppliers for Brand |
| Purchases | `GET /purchases/by-brand/{brandId}` | Purchases for Brand |
| Warehouses | `GET /warehouses/by-brand/{brandId}` | Warehouses for Brand |
| WarehouseBatches | `GET /warehousebatches/by-brand/{brandId}` | Batches for Brand |
| Batches | `GET /batches/by-brand/{brandId}` | Batches for Brand |
| StockMovements | `GET /stockmovements/by-brand/{brandId}` | Movements for Brand |
| Order | `GET /order/by-brand/{brandId}` | Orders for Brand |
| OrderItems | `GET /orderitems/by-brand/{brandId}` | Items for Brand |

---

## Example Requests

### Get all employees for Brand ABC123
```bash
curl -X GET "https://api.example.com/employees/by-brand/550e8400-e29b-41d4-a716-446655440000" \
  -H "accept: application/json"
```

### Get all purchases for Brand XYZ789
```bash
curl -X GET "https://api.example.com/purchases/by-brand/650e8400-e29b-41d4-a716-446655440001" \
  -H "accept: application/json"
```

### Get all warehouse batches for Brand DEF456
```bash
curl -X GET "https://api.example.com/warehousebatches/by-brand/750e8400-e29b-41d4-a716-446655440002" \
  -H "accept: application/json"
```

---

## Query Handlers

Each endpoint is backed by a query handler in the Application layer:

```
Application/Queries/[Entity]/GetByBrandId/GetAll[Entity]sByBrandIdQuery.cs
```

Example structure:
```csharp
public class GetAllProductsByBrandIdQuery : IRequest<Response<IEnumerable<ProductDto>>>
{
    public Guid BrandId { get; set; }
    public GetAllProductsByBrandIdQuery(Guid brandId) => BrandId = brandId;
}
```

---

## Response Format

All endpoints return the standard response wrapper:

```json
{
  "statusCode": 200,
  "succeeded": true,
  "message": "Success",
  "data": [
    // Array of filtered items
  ],
  "error": []
}
```

---

## HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success - Items found and returned |
| 400 | Bad Request - Invalid BrandId format |
| 500 | Server Error - Unexpected error |

---

## Implementation Notes

? All 15 controllers updated with brand-filtering endpoints  
? 14 new query handlers created in Application layer  
? Uses existing repository methods (`GetAllByBrandIdAsync`)  
? Consistent route pattern: `/by-brand/{brandId:guid}`  
? Full OpenAPI/Swagger documentation support  
? Build verified - No compilation errors  

---

## Quick Navigation

**Entities with Brand Filtering:**
- Products & ProductCategories
- Branches
- Employees
- Customers
- Expenses & ExpenseCategories
- Suppliers
- Purchases & PurchaseItems
- Warehouses & WarehouseBatches
- Batches
- StockMovements
- Orders & OrderItems

---

## Testing

Test with real BrandId values from your database:

1. Get a Brand ID from the database
2. Call any endpoint with format: `/[resource]/by-brand/{brandId}`
3. Verify filtered results are returned
4. Confirm all items have the correct BrandId

---

## Architecture

```
API Controller
    ?
MediatR Query
    ?
Query Handler
    ?
Repository Method (GetAllByBrandIdAsync)
    ?
Database Query
```

All handlers follow this Clean Architecture pattern consistently.
