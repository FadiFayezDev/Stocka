using BuildingBlocks.Application.Abstractions;
using Catalog.Infrastructure;
using Core.Infrastructure;
using Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Stocka.MVC.Infrastructure;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantContext, HttpTenantContext>();
builder.Services.AddHealthChecks();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

#region Modules
var configuration = builder.Configuration;
builder.Services.AddInventoryModule(configuration);
builder.Services.AddCatalogModule(configuration);
builder.Services.AddCoreModule(configuration);
#endregion


var app = builder.Build();

// Auto-migrate databases on startup (development only — use with care in production)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    try
    {
        var catalogCtx = scope.ServiceProvider.GetService<Catalog.Infrastructure.Contexts.CatalogDbContext>();
        if (catalogCtx != null) await catalogCtx.Database.MigrateAsync();

        var inventoryCtx = scope.ServiceProvider.GetService<Inventory.Infrastructure.Contexts.InventoryDbContext>();
        if (inventoryCtx != null) await inventoryCtx.Database.MigrateAsync();

        var coreCtx = scope.ServiceProvider.GetService<Core.Infrastructure.Contexts.CoreDbContext>();
        if (coreCtx != null) await coreCtx.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
var supportedCultures = new[] { new CultureInfo("ar"), new CultureInfo("en") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ar"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapHealthChecks("/health");



app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
