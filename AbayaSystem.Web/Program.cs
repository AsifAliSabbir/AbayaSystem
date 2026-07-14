using AbayaSystem.Core;
using AbayaSystem.Infrastructure;
using AbayaSystem.Web;
using AbayaSystem.Web.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization; // 👈 Required namespace added

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Wire up your Application Order Engine service dependency
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddDbContext<BoutiqueDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();

// Registers core auth services
builder.Services.AddAuthenticationCore();
builder.Services.AddAuthorizationCore();

// 🚨 THE CRITICAL FIX: Directs initial HTTP requests to Blazor instead of failing on challenge
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();

// Register our custom provider instance inside the Scoped circuit container
builder.Services.AddScoped<BoutiqueAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<BoutiqueAuthStateProvider>());

// Activate the framework's cascading state tracking infrastructure
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Create a temporary scope to resolve dependencies cleanly during application startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<BoutiqueDbContext>();
        // Run the seeding logic directly into your SQL Server instance
        await BoutiqueDbContext.SeedDatabaseAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the SQL Server database tables.");
    }
}

app.Run();