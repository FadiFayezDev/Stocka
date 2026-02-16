# ??? ??? Domain Models ?????? - New Pattern Overview

## ?? ????? ?????? ??? Entities

???? ??? Entities ???? ???? ??? ???? ????:

```csharp
public class EntityName : IEntity<Guid>
{
    // ============ Properties ============
    
    // 1. Key Properties (?? public setter ??? EF Core)
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    
    // 2. Value Properties 
    public string Name { get; set; } = null!;
    
    // 3. State Properties (?? ???? private setter ??? Properties ??????)
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; set; }
    
    // 4. Collections (read-only ?? ??????)
    private readonly List<ChildEntity> _children = new();
    public virtual ICollection<ChildEntity> Children => _children.AsReadOnly();
    
    // ============ Navigation Properties ============
    public virtual Brand Brand { get; set; } = null!;
    
    // ============ Constructors ============
    
    // Private parameterless constructor (??? EF Core)
    private EntityName() { }
    
    // Public constructor ?? validation
    public EntityName(Guid parentId, string name, decimal amount)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.", nameof(amount));
        
        // Initialization
        ParentId = parentId;
        Name = name.Trim();
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
    }
    
    // ============ Business Logic Methods ============
    
    // 1. Update Methods ?? validation
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty.", nameof(newName));
        
        Name = newName.Trim();
    }
    
    public void UpdateAmount(decimal newAmount)
    {
        if (newAmount <= 0)
            throw new ArgumentException("Amount must be positive.", nameof(newAmount));
        
        Amount = newAmount;
    }
    
    // 2. Collection Management Methods
    public void AddChild(ChildEntity child)
    {
        if (child == null)
            throw new ArgumentNullException(nameof(child));
        
        if (child.ParentId != Id)
            throw new ArgumentException("Child does not belong to this entity.");
        
        if (_children.Any(c => c.Id == child.Id))
            throw new InvalidOperationException("Child already exists.");
        
        _children.Add(child);
    }
    
    public void RemoveChild(Guid childId)
    {
        var child = _children.FirstOrDefault(c => c.Id == childId);
        if (child == null)
            throw new ArgumentException("Child not found.");
        
        _children.Remove(child);
    }
    
    // 3. Query Methods
    public bool HasChildren => _children.Any();
    
    public ChildEntity? GetChild(Guid childId)
        => _children.FirstOrDefault(c => c.Id == childId);
    
    // ============ IEntity Implementation ============
    public Guid GetKey() => Id;
    
    public void SetKey(Guid key) => Id = key;
}
```

---

## ?? ???? ????: Purchase Entity

```csharp
public class Purchase : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid BrandId { get; set; }
    public Guid SupplierId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal TotalAmount { get; private set; }
    
    private readonly List<PurchaseItem> _purchaseItems = new();
    
    public virtual Brand Brand { get; set; } = null!;
    public virtual ICollection<PurchaseItem> PurchaseItems => _purchaseItems.AsReadOnly();
    public virtual Supplier Supplier { get; set; } = null!;
    
    private Purchase() { }
    
    // Constructor ?? validation
    public Purchase(Guid brandId, Guid supplierId, DateTime? purchaseDate = null)
    {
        BrandId = brandId;
        SupplierId = supplierId;
        PurchaseDate = purchaseDate ?? DateTime.UtcNow;
        TotalAmount = 0;
    }
    
    // ????? item ?? validation ? recalculation
    public void AddPurchaseItem(PurchaseItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        
        if (item.PurchaseId != Id)
            throw new ArgumentException("Item does not belong to this purchase.");
        
        if (_purchaseItems.Any(pi => pi.Id == item.Id))
            throw new InvalidOperationException("Item already added.");
        
        _purchaseItems.Add(item);
        RecalculateTotal();
    }
    
    // ??? item ?? recalculation
    public void RemovePurchaseItem(Guid itemId)
    {
        var item = _purchaseItems.FirstOrDefault(pi => pi.Id == itemId);
        if (item == null)
            throw new ArgumentException("Item not found.");
        
        _purchaseItems.Remove(item);
        RecalculateTotal();
    }
    
    // Private helper method
    private void RecalculateTotal()
    {
        TotalAmount = _purchaseItems.Sum(pi => pi.TotalCost);
    }
    
    public Guid GetKey() => Id;
    public void SetKey(Guid key) => Id = key;
}
```

---

## ?? ????? ??? ??????? ?????? ???????

### ? ?????? (Weak Domain)
```csharp
// Entity ???? logic
public class Purchase
{
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; } // ?? ???? ????
    public List<PurchaseItem> Items { get; set; } // ???? ?????? ?????
}

// Logic ?? Application
var purchase = new Purchase { TotalAmount = -100 }; // ?? ???? validation!
if (request.Items.Count == 0) throw new Exception("..."); // validation ?? layer ????
purchase.TotalAmount = request.Items.Sum(i => i.Cost); // recalculation manual
```

### ? ?????? (Strong Domain)
```csharp
// Entity ??? ?? logic
public class Purchase
{
    private readonly List<PurchaseItem> _items = new();
    public IReadOnlyCollection<PurchaseItem> Items => _items.AsReadOnly();
    public decimal TotalAmount { get; private set; } // ????
    
    public Purchase(Guid brandId, Guid supplierId) { }
    
    public void AddItem(PurchaseItem item)
    {
        // Validation
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (_items.Any(i => i.Id == item.Id)) throw new InvalidOperationException("...");
        
        _items.Add(item);
        RecalculateTotal(); // Automatic recalculation
    }
}

// ??????? ???? ????
var purchase = new Purchase(brandId, supplierId); // Valid from the start
purchase.AddItem(item); // Validates & recalculates automatically
```

---

## ??? Best Practices ???????

### 1?? **Immutability of Collection References**
```csharp
// ? ????
public List<Item> Items { get; set; }

// ? ???
private readonly List<Item> _items = new();
public IReadOnlyCollection<Item> Items => _items.AsReadOnly();
```

### 2?? **Validation in Constructor**
```csharp
public class Expense : IEntity<Guid>
{
    public Expense(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");
        // ...
    }
}
```

### 3?? **Business Operations via Methods**
```csharp
// ? ????
sale.TotalAmount = newTotal; // ?? ???? ??? ????

// ? ???
sale.UpdateTotal(newTotal); // ????? ?? ????? ?????
```

### 4?? **State Validation**
```csharp
public void CompleteSale()
{
    if (Status == SaleStatus.Completed)
        throw new InvalidOperationException("Sale already completed.");
    
    if (!_items.Any())
        throw new InvalidOperationException("Sale must have items.");
    
    Status = SaleStatus.Completed;
}
```

### 5?? **Helper Query Methods**
```csharp
public decimal GetTotalCost() => Items.Sum(i => i.Cost);
public bool IsCompleted => Status == SaleStatus.Completed;
public bool HasItems => Items.Any();
```

---

## ?? ?????? ?????

| ?????? | ??????? ?????? | ??????? ?????? |
|-------|---|---|
| Validation | ?? Application/API | ?? Domain Entity |
| Business Rules | ????? ?? Application | ????? ?? Domain |
| Invariants | ?? ?????? | ?????? ???????? |
| Collection Safety | ???? ????? | ????? |
| Maintainability | ??? | ??? |
| Testability | ????? mocking | ???? ??????? ?????? |
| Code Duplication | ????? | ?????? |

---

## ?? ??????? ???????

> **Clean Architecture Principle**: 
> 
> ??? Domain Layer ??? ?? ????? ??? **?? ??? Business Rules**? 
> ??? ???? ???? **data containers**.

??? Entities ????:
- ? ????? ??? integrity ????????
- ? ???? Business Rules ????????
- ? ???? testable ??????
- ? ???? ???? ???? ??? Business Logic
- ? ???? ????? ???????

---

## ?? ?????? ???????

???? ??? ?????:
1. **Application Layer**: ??????? methods ??????? ????? ?? property assignment
2. **API Layer**: ???????? ??? validation ??? Domain ????? ?? Application
3. **Unit Tests**: ?????? ??? Business Rules ?? Domain
4. **Domain Events**: ????? events ???????? ??????
