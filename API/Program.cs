using System.Text;
using API.Filters;
using Application;
using Application.Common.Security;
using CleanArchitecture.Api.Middleware;
using Domain.Enums;
using Infrastructure;
using Infrastructure.Contexts;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<EnforceActiveBrandRouteFilter>();
});
builder.Services.AddScoped<EnforceActiveBrandRouteFilter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddApplicationRegisteration();
builder.Services.AddInfrastructureRegistration(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new ArgumentNullException(nameof(connectionString), "Connection string is missing");

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? throw new InvalidOperationException("JWT key is missing")))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy(BrandPolicies.SalesCreate, policy =>
        policy.RequireAssertion(ctx => HasBrandRole(ctx,
            BrandRole.Owner, BrandRole.BrandAdmin, BrandRole.Manager, BrandRole.Cashier)));

    options.AddPolicy(BrandPolicies.ProductsManage, policy =>
        policy.RequireAssertion(ctx => HasBrandRole(ctx,
            BrandRole.Owner, BrandRole.BrandAdmin, BrandRole.Manager)));

    options.AddPolicy(BrandPolicies.PurchasesManage, policy =>
        policy.RequireAssertion(ctx => HasBrandRole(ctx,
            BrandRole.Owner, BrandRole.BrandAdmin, BrandRole.Manager)));

    options.AddPolicy(BrandPolicies.BranchEmployeesManage, policy =>
        policy.RequireAssertion(ctx => HasBrandRole(ctx,
            BrandRole.Owner, BrandRole.BrandAdmin, BrandRole.Manager)));

    options.AddPolicy(BrandPolicies.ReportsBrandWide, policy =>
        policy.RequireAssertion(ctx => HasBrandRole(ctx,
            BrandRole.Owner, BrandRole.BrandAdmin)));
});

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins, policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:3002", "http://localhost:3003")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        policy
            .WithOrigins("https://stocka-ui.vercel.app")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(myAllowSpecificOrigins);

if (!app.Environment.IsDevelopment())
{
    app.UseMiddleware<GlobalExceptionMiddleware>();
}

// app.UseHttpsRedirection(); // تعطيل مؤقتاً للتطوير
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();
app.Run();

static bool HasBrandRole(AuthorizationHandlerContext context, params BrandRole[] allowedRoles)
{
    var roleClaim = context.User.FindFirst(ClaimNames.BrandRole)?.Value;
    if (!Enum.TryParse<BrandRole>(roleClaim, true, out var role))
        return false;

    return allowedRoles.Contains(role);
}
