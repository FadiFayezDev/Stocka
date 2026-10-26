using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Primitives.Identifiers;
using Inventory.Contracts.UseCases.BatchCases;
using Inventory.Contracts.UseCases.StockCases;
using Inventory.Contracts.UseCases.WarehouseCases;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Integration.Tests;

/// <summary>
/// Tenant context that tests can switch per scope. Scopes that use the same
/// BrandId share brand data; a different BrandId is a different tenant entirely.
/// </summary>
public sealed class TestTenantContext : ITenantContext
{
    public BrandId BrandId { get; set; } = BrandId.New();
    public Guid? UserId => null;
}

public sealed class InventoryScope : IDisposable
{
    private readonly IServiceScope _scope;

    public InventoryScope(IServiceScope scope, BrandId? brandId = null)
    {
        _scope = scope;
        Tenant = (TestTenantContext)scope.ServiceProvider.GetRequiredService<ITenantContext>();
        if (brandId != null)
            Tenant.BrandId = brandId!.Value;

        Sender = scope.ServiceProvider.GetRequiredService<ISender>();
        Db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

        WarehouseCases = scope.ServiceProvider.GetRequiredService<IWarehouseCases>();
        BatchCases = scope.ServiceProvider.GetRequiredService<IBatchCases>();
        StockCases = scope.ServiceProvider.GetRequiredService<IStockCases>();
    }

    public TestTenantContext Tenant { get; }
    public ISender Sender { get; }
    public InventoryDbContext Db { get; }

    /// <summary>Pure contract use-case boundaries resolved from the scope's DI container.</summary>
    public IWarehouseCases WarehouseCases { get; }
    public IBatchCases BatchCases { get; }
    public IStockCases StockCases { get; }

    public void Dispose() => _scope.Dispose();
}

/// <summary>
/// One SQL Server (LocalDB) database per test run, migrated to the latest schema, shared by
/// every Inventory integration test (real relational behavior: constraints, RowVersion,
/// global query filters — not an EF InMemory fake).
/// </summary>
public sealed class InventoryDatabaseFixture : IAsyncLifetime
{
    private static readonly string DatabaseName = $"StockaInventoryTests_{Guid.NewGuid():N}";

    private readonly ServiceProvider _provider;
    private readonly string _connectionString =
        $@"Server=(localdb)\MSSQLLocalDB;Database={DatabaseName};Trusted_Connection=True;MultipleActiveResultSets=True;";

    public InventoryDatabaseFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _connectionString
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddInventoryModule(configuration);
        services.AddScoped<ITenantContext, TestTenantContext>();

        _provider = services.BuildServiceProvider();
    }

    public async Task InitializeAsync()
    {
        var db = _provider.GetRequiredService<InventoryDbContext>();
        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();
    }

    public InventoryScope NewScope(BrandId? brandId = null) =>
        new(_provider.CreateScope(), brandId);

    public async Task DisposeAsync()
    {
        try
        {
            using var scope = NewScope();
            await scope.Db.Database.EnsureDeletedAsync();
        }
        catch
        {
            // Best effort cleanup; the test database name is unique per run anyway.
        }

        await _provider.DisposeAsync();
    }
}

[CollectionDefinition("InventoryDatabase")]
public sealed class InventoryDatabaseCollection : ICollectionFixture<InventoryDatabaseFixture>
{
}
