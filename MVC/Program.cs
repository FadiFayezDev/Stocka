using Application;
using Application.Common.Security;
using Infrastructure;
using Infrastructure.Contexts;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Health Checks
builder.Services.AddHealthChecks();

// Application + Infrastructure
builder.Services.AddApplicationRegisteration();
builder.Services.AddInfrastructureRegistration(builder.Configuration);

// Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Identity Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = false;
});

builder.Services.AddAuthorization();

// JWT-cookie authentication
const string JwtOrIdentityScheme = "JwtOrIdentity";
const string AccessTokenCookieName = "access_token";

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtOrIdentityScheme;
    options.DefaultAuthenticateScheme = JwtOrIdentityScheme;
    options.DefaultChallengeScheme = JwtOrIdentityScheme;
})
.AddPolicyScheme(JwtOrIdentityScheme, "JWT or Identity", options =>
{
    options.ForwardDefaultSelector = context =>
        context.Request.Cookies.ContainsKey(AccessTokenCookieName)
            ? JwtBearerDefaults.AuthenticationScheme
            : IdentityConstants.ApplicationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"]
                ?? throw new InvalidOperationException("JWT key is missing")))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies[AccessTokenCookieName];
            return Task.CompletedTask;
        },

        OnTokenValidated = context =>
        {
            var identity = (ClaimsIdentity)context.Principal!.Identity!;

            var userId = context.Principal.FindFirstValue(ClaimNames.UserId);
            if (!string.IsNullOrEmpty(userId))
            {
                var nameIdentifier = identity.FindFirst(ClaimTypes.NameIdentifier);
                if (nameIdentifier != null)
                    identity.RemoveClaim(nameIdentifier);

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            }

            var userName = context.Principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!string.IsNullOrEmpty(userName) && identity.FindFirst(ClaimTypes.Name) == null)
                identity.AddClaim(new Claim(ClaimTypes.Name, userName));

            return Task.CompletedTask;
        },

        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.Cookies.Delete(AccessTokenCookieName);

            if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Task.CompletedTask;

            var returnUrl = context.Request.Path + context.Request.QueryString;
            var loginUrl = string.IsNullOrEmpty(returnUrl)
                ? "/Identity/Account/Login"
                : $"/Identity/Account/Login?returnUrl={Uri.EscapeDataString(returnUrl)}";

            context.Response.Redirect(loginUrl);
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Auto-Migration on Startup (Fail-fast strategy)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        logger.LogInformation("Applying database migrations...");
        await dbContext.Database.MigrateAsync();
        logger.LogInformation("Database migrations applied successfully!");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "FATAL: An error occurred while applying database migrations.");
        throw; // إيقاف التطبيق فوراً لرؤية السبب في docker logs وعدم كتم الاستثناء
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<MVC.Middleware.LocalizationMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapHealthChecks("/health")
   .AllowAnonymous();

app.Run();