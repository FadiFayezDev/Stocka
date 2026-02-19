# API Controllers Documentation

## Overview
Complete REST API controllers for all 20 entities in the Stocka application. Each controller provides full CRUD operations with proper HTTP methods and status codes.

## Created Controllers (20 Total)

### 1. Accounts Controller
**File**: `API\Controllers\AccountsController.cs`
**Route**: `/api/accounts`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/accounts` | Get all accounts |
| GET | `/api/accounts/{id}` | Get account by ID |
| POST | `/api/accounts` | Create new account |
| PUT | `/api/accounts/{id}` | Update account |
| DELETE | `/api/accounts/{id}` | Delete account |

### 2. Brands Controller
**File**: `API\Controllers\BrandsController.cs`
**Route**: `/api/brands`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/brands` | Get all brands |
| GET | `/api/brands/{id}` | Get brand by ID |
| POST | `/api/brands` | Create new brand |
| PUT | `/api/brands/{id}` | Update brand |
| DELETE | `/api/brands/{id}` | Delete brand |

### 3. Branches Controller
**File**: `API\Controllers\BranchesController.cs`
**Route**: `/api/branches`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/branches` | Get all branches |
| GET | `/api/branches/{id}` | Get branch by ID |
| POST | `/api/branches` | Create new branch |
| PUT | `/api/branches/{id}` | Update branch |
| DELETE | `/api/branches/{id}` | Delete branch |

### 4. Employees Controller
**File**: `API\Controllers\EmployeesController.cs`
**Route**: `/api/employees`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/employees` | Get all employees |
| GET | `/api/employees/{id}` | Get employee by ID |
| POST | `/api/employees` | Create new employee |
| PUT | `/api/employees/{id}` | Update employee |
| DELETE | `/api/employees/{id}` | Delete employee |

### 5. Customers Controller
**File**: `API\Controllers\CustomersController.cs`
**Route**: `/api/customers`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/customers` | Get all customers |
| GET | `/api/customers/{id}` | Get customer by ID |
| POST | `/api/customers` | Create new customer |
| PUT | `/api/customers/{id}` | Update customer |
| DELETE | `/api/customers/{id}` | Delete customer |

### 6. Warehouses Controller
**File**: `API\Controllers\WarehousesController.cs`
**Route**: `/api/warehouses`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/warehouses` | Get all warehouses |
| GET | `/api/warehouses/{id}` | Get warehouse by ID |
| POST | `/api/warehouses` | Create new warehouse |
| PUT | `/api/warehouses/{id}` | Update warehouse |
| DELETE | `/api/warehouses/{id}` | Delete warehouse |

### 7. Products Controller
**File**: `API\Controllers\ProductsController.cs`
**Route**: `/api/products`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create new product |
| PUT | `/api/products/{id}` | Update product |
| DELETE | `/api/products/{id}` | Delete product |

### 8. Product Categories Controller
**File**: `API\Controllers\ProductCategoriesController.cs`
**Route**: `/api/productCategories`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/productCategories` | Get all product categories |
| GET | `/api/productCategories/{id}` | Get product category by ID |
| POST | `/api/productCategories` | Create new product category |
| PUT | `/api/productCategories/{id}` | Update product category |
| DELETE | `/api/productCategories/{id}` | Delete product category |

### 9. Suppliers Controller
**File**: `API\Controllers\SuppliersController.cs`
**Route**: `/api/suppliers`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/suppliers` | Get all suppliers |
| GET | `/api/suppliers/{id}` | Get supplier by ID |
| POST | `/api/suppliers` | Create new supplier |
| PUT | `/api/suppliers/{id}` | Update supplier |
| DELETE | `/api/suppliers/{id}` | Delete supplier |

### 10. Purchases Controller
**File**: `API\Controllers\PurchasesController.cs`
**Route**: `/api/purchases`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/purchases` | Get all purchases |
| GET | `/api/purchases/{id}` | Get purchase by ID |
| POST | `/api/purchases` | Create new purchase |
| PUT | `/api/purchases/{id}` | Update purchase |
| DELETE | `/api/purchases/{id}` | Delete purchase |

### 11. Purchase Items Controller
**File**: `API\Controllers\PurchaseItemsController.cs`
**Route**: `/api/purchaseItems`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/purchaseItems` | Get all purchase items |
| GET | `/api/purchaseItems/{id}` | Get purchase item by ID |
| POST | `/api/purchaseItems` | Create new purchase item |
| PUT | `/api/purchaseItems/{id}` | Update purchase item |
| DELETE | `/api/purchaseItems/{id}` | Delete purchase item |

### 12. Batches Controller
**File**: `API\Controllers\BatchesController.cs`
**Route**: `/api/batches`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/batches` | Get all batches |
| GET | `/api/batches/{id}` | Get batch by ID |
| POST | `/api/batches` | Create new batch |
| PUT | `/api/batches/{id}` | Update batch |
| DELETE | `/api/batches/{id}` | Delete batch |

### 13. Warehouse Batches Controller
**File**: `API\Controllers\WarehouseBatchesController.cs`
**Route**: `/api/warehouseBatches`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/warehouseBatches` | Get all warehouse batches |
| GET | `/api/warehouseBatches/{id}` | Get warehouse batch by ID |
| POST | `/api/warehouseBatches` | Create new warehouse batch |
| PUT | `/api/warehouseBatches/{id}` | Update warehouse batch |
| DELETE | `/api/warehouseBatches/{id}` | Delete warehouse batch |

### 14. Expenses Controller
**File**: `API\Controllers\ExpensesController.cs`
**Route**: `/api/expenses`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/expenses` | Get all expenses |
| GET | `/api/expenses/{id}` | Get expense by ID |
| POST | `/api/expenses` | Create new expense |
| PUT | `/api/expenses/{id}` | Update expense |
| DELETE | `/api/expenses/{id}` | Delete expense |

### 15. Expense Categories Controller
**File**: `API\Controllers\ExpenseCategoriesController.cs`
**Route**: `/api/expenseCategories`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/expenseCategories` | Get all expense categories |
| GET | `/api/expenseCategories/{id}` | Get expense category by ID |
| POST | `/api/expenseCategories` | Create new expense category |
| PUT | `/api/expenseCategories/{id}` | Update expense category |
| DELETE | `/api/expenseCategories/{id}` | Delete expense category |

### 16. Stock Movements Controller
**File**: `API\Controllers\StockMovementsController.cs`
**Route**: `/api/stockMovements`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/stockMovements` | Get all stock movements |
| GET | `/api/stockMovements/{id}` | Get stock movement by ID |
| POST | `/api/stockMovements` | Create new stock movement |
| PUT | `/api/stockMovements/{id}` | Update stock movement |
| DELETE | `/api/stockMovements/{id}` | Delete stock movement |

### 17. Sales Controller
**File**: `API\Controllers\SalesController.cs`
**Route**: `/api/sales`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/sales` | Get all sales |
| GET | `/api/sales/{id}` | Get sale by ID |
| POST | `/api/sales` | Create new sale |
| PUT | `/api/sales/{id}` | Update sale |
| DELETE | `/api/sales/{id}` | Delete sale |

### 18. Sale Items Controller
**File**: `API\Controllers\SaleItemsController.cs`
**Route**: `/api/saleItems`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/saleItems` | Get all sale items |
| GET | `/api/saleItems/{id}` | Get sale item by ID |
| POST | `/api/saleItems` | Create new sale item |
| PUT | `/api/saleItems/{id}` | Update sale item |
| DELETE | `/api/saleItems/{id}` | Delete sale item |

### 19. Journal Entries Controller
**File**: `API\Controllers\JournalEntriesController.cs`
**Route**: `/api/journalEntries`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/journalEntries` | Get all journal entries |
| GET | `/api/journalEntries/{id}` | Get journal entry by ID |
| POST | `/api/journalEntries` | Create new journal entry |
| PUT | `/api/journalEntries/{id}` | Update journal entry |
| DELETE | `/api/journalEntries/{id}` | Delete journal entry |

### 20. Journal Entry Lines Controller
**File**: `API\Controllers\JournalEntryLinesController.cs`
**Route**: `/api/journalEntryLines`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/journalEntryLines` | Get all journal entry lines |
| GET | `/api/journalEntryLines/{id}` | Get journal entry line by ID |
| POST | `/api/journalEntryLines` | Create new journal entry line |
| PUT | `/api/journalEntryLines/{id}` | Update journal entry line |
| DELETE | `/api/journalEntryLines/{id}` | Delete journal entry line |

## HTTP Status Codes

All controllers follow standard HTTP status codes:

| Status Code | Meaning |
|-------------|---------|
| 200 OK | Request successful (for GET, PUT operations) |
| 201 Created | Resource successfully created (for POST operations) |
| 400 Bad Request | Invalid request or validation failed |
| 404 Not Found | Resource not found |
| 500 Internal Server Error | Server error occurred |

## Request/Response Format

### Successful Response (200/201)
```json
{
  "succeeded": true,
  "data": {
    "id": "guid",
    "name": "value",
    // ... entity properties
  },
  "message": "Created Successfully",
  "statusCode": 201
}
```

### Error Response (400/404)
```json
{
  "succeeded": false,
  "data": null,
  "message": "Entity not found",
  "statusCode": 404
}
```

## Features

? **Complete CRUD Operations** - Create, Read, Update, Delete for all entities
? **Validation** - ModelState validation for all requests
? **Error Handling** - Proper error responses with HTTP status codes
? **ID Validation** - Ensures URL ID matches request body ID
? **Cancellation Tokens** - Support for async operation cancellation
? **Response Types** - Proper ProducesResponseType attributes for documentation
? **Base Controller** - Inherits from BaseController with IMediator
? **RESTful Design** - Follows REST conventions

## Usage Example

### Create a Product
```
POST /api/products
Content-Type: application/json

{
  "brandId": "guid",
  "categoryId": "guid",
  "name": "Product Name",
  "barcode": "123456"
}

Response:
201 Created
{
  "succeeded": true,
  "data": {
    "id": "guid",
    "brandId": "guid",
    "categoryId": "guid",
    "name": "Product Name",
    "barcode": "123456"
  },
  "message": "Created Successfully",
  "statusCode": 201
}
```

### Update a Product
```
PUT /api/products/{id}
Content-Type: application/json

{
  "id": "guid",
  "brandId": "guid",
  "categoryId": "guid",
  "name": "Updated Name",
  "barcode": "654321"
}

Response:
200 OK
{
  "succeeded": true,
  "data": {
    "id": "guid",
    "brandId": "guid",
    "categoryId": "guid",
    "name": "Updated Name",
    "barcode": "654321"
  },
  "message": "Updated Successfully",
  "statusCode": 200
}
```

### Delete a Product
```
DELETE /api/products/{id}

Response:
200 OK
{
  "succeeded": true,
  "data": true,
  "message": "Deleted Successfully",
  "statusCode": 200
}
```

## Integration with Handlers

Each controller endpoint calls the appropriate handler through MediatR:

- **GET all** ? Calls GetAllXxxQuery
- **GET by ID** ? Calls GetXxxByIdQuery
- **POST** ? Calls CreateXxxCommand
- **PUT** ? Calls UpdateXxxCommand
- **DELETE** ? Calls DeleteXxxCommand

The handlers manage:
- Transaction management
- Data persistence
- Error handling
- DTO mapping

## Notes

1. All controllers validate ModelState before processing
2. ID mismatches in PUT requests return Bad Request
3. CancellationToken is passed through the entire pipeline
4. All responses are wrapped in Response<T> pattern
5. Controllers inherit from BaseController which provides IMediator access
