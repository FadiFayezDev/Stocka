# ? ?????? ????: BrandUser Configuration

## ?? ????? ????????

### 1. ????? Build
```bash
dotnet build
```
? **?????:** Build successful

---

### 2. ????? Migration
```powershell
# ?? Package Manager Console
Add-Migration AddBrandUserConfiguration
```

**???????:**
- ? ????? Migration file ????
- ? ???? errors
- ? ????? ??? ???? BrandUsers

---

### 3. ????? Migration
```powershell
Update-Database
```

**???????:**
- ? ????? ???? errors
- ? ???? BrandUsers ??? ??????

---

### 4. ?????? ?? Database

#### ?? SQL Server Management Studio:

```sql
-- 1. ?????? ?? ???? ??????
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'BrandUsers'
GO

-- 2. ?????? ?? Columns
SELECT COLUMN_NAME, IS_NULLABLE, DATA_TYPE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'BrandUsers'
ORDER BY ORDINAL_POSITION
GO

-- 3. ?????? ?? Primary Key
SELECT * 
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'BrandUsers' 
  AND CONSTRAINT_NAME LIKE 'PK%'
GO

-- 4. ?????? ?? Foreign Keys
SELECT * 
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
WHERE TABLE_NAME = 'BrandUsers'
GO

-- 5. ????? ?????? ??????
INSERT INTO BrandUsers (BrandId, UserId, Role)
VALUES (NEWID(), NEWID(), 'Manager')
GO

-- 6. ????????? ?? ????????
SELECT TOP 1 * FROM BrandUsers
GO
```

---

## ?? ???????? ??? Unit

### ?????? ??????? ???????

```csharp
[TestClass]
public class BrandUserConfigurationTests
{
    [TestMethod]
    public void BrandUser_ShouldHaveCompositeKey()
    {
        // Arrange & Act
        var dbContext = new AppDbContext(GetOptions());
        var brandUserKey = dbContext.Model
            .FindEntityType(typeof(BrandUser))?
            .FindPrimaryKey();

        // Assert
        Assert.IsNotNull(brandUserKey);
        Assert.AreEqual(2, brandUserKey.Properties.Count);
        Assert.IsTrue(brandUserKey.Properties
            .Any(p => p.Name == "BrandId"));
        Assert.IsTrue(brandUserKey.Properties
            .Any(p => p.Name == "UserId"));
    }

    [TestMethod]
    public void BrandUser_ShouldHaveBrandRelationship()
    {
        // Arrange & Act
        var dbContext = new AppDbContext(GetOptions());
        var brandUserType = dbContext.Model
            .FindEntityType(typeof(BrandUser));

        // Assert
        var brandNavigation = brandUserType?
            .FindNavigation("Brand");
        Assert.IsNotNull(brandNavigation);
    }

    [TestMethod]
    public void BrandUser_ShouldHaveUserRelationship()
    {
        // Arrange & Act
        var dbContext = new AppDbContext(GetOptions());
        var brandUserType = dbContext.Model
            .FindEntityType(typeof(BrandUser));

        // Assert
        var userNavigation = brandUserType?
            .FindNavigation("User");
        Assert.IsNotNull(userNavigation);
    }

    [TestMethod]
    public void BrandUser_RoleShouldBeRequired()
    {
        // Arrange & Act
        var dbContext = new AppDbContext(GetOptions());
        var roleProperty = dbContext.Model
            .FindEntityType(typeof(BrandUser))?
            .FindProperty("Role");

        // Assert
        Assert.IsNotNull(roleProperty);
        Assert.IsFalse(roleProperty.IsNullable);
    }

    private DbContextOptions<AppDbContext> GetOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }
}
```

---

## ?? ???????? ??? Integration

### ?????? ??? CRUD Operations

```csharp
[TestClass]
public class BrandUserIntegrationTests
{
    private AppDbContext _context;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
    }

    [TestMethod]
    public async Task AddBrandUser_ShouldInsertSuccessfully()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var brandUser = new BrandUser
        {
            BrandId = brandId,
            UserId = userId,
            Role = "Manager"
        };

        // Act
        _context.BrandUsers.Add(brandUser);
        await _context.SaveChangesAsync();

        // Assert
        var savedBrandUser = await _context.BrandUsers
            .FirstOrDefaultAsync(bu => 
                bu.BrandId == brandId && bu.UserId == userId);
        
        Assert.IsNotNull(savedBrandUser);
        Assert.AreEqual("Manager", savedBrandUser.Role);
    }

    [TestMethod]
    public async Task BrandUser_ShouldNotAllowDuplicateCompositeKey()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var brandUser1 = new BrandUser
        {
            BrandId = brandId,
            UserId = userId,
            Role = "Manager"
        };
        var brandUser2 = new BrandUser
        {
            BrandId = brandId,
            UserId = userId,
            Role = "Cashier"
        };

        // Act & Assert
        _context.BrandUsers.Add(brandUser1);
        await _context.SaveChangesAsync();

        _context.BrandUsers.Add(brandUser2);
        
        var ex = await Assert.ThrowsExceptionAsync<DbUpdateException>(
            async () => await _context.SaveChangesAsync());
        
        Assert.IsNotNull(ex);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context?.Dispose();
    }
}
```

---

## ? ???? ??????

### ??? ????????:
- [ ] Build ? Successful
- [ ] ?? ???? CS errors
- [ ] Migration ???? ???? ?????
- [ ] Update-Database ?? ?????
- [ ] ?????? ????? ?? Database
- [ ] Foreign Keys ??????
- [ ] Primary Key ????? ?? Composite

### ??? ????????:
- [ ] ?? ????? ??? Unit Tests
- [ ] ?? ????? ??? Integration Tests
- [ ] ???? ?????????? ????
- [ ] ???? ????? ?????? ????????

---

## ?? ???????

? **???????:** BrandUser ???? Primary Key  
? **????:** ????? Composite Key ? Configuration  
? **???????:** Build Successful ? Database Ready  
? **??????:** ???? ??? Production
