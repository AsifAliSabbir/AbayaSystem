using AbayaSystem.Core;
using AbayaSystem.Infrastructure;
using AbayaSystem.Web;
using AbayaSystem.Web.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization; // 👈 Required namespace added
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


//string conl = @"Server=db59869.public.databaseasp.net; Database=db59869; User Id=db59869; Password=Cy8+?Ga3h%2T; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True; ";


//string conl2 = @"Server=db59869.databaseasp.net; Database=db59869; User Id=db59869; Password=Cy8+?Ga3h%2T; Encrypt=False; MultipleActiveResultSets=True;";


string currentConnectionString = GlobalFunctions.conl; // Change this to the desired connection string
//string currentConnectionString = conl; // Change this to the desired connection string

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Wire up your Application Order Engine service dependency
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IWorkerService, WorkerService>();
builder.Services.AddScoped<IFabricManagementService, FabricManagementService>();

builder.Services.AddScoped<IExternalWorkerService, ExternalWorkerService>();


//string con3 = @"Server=	AbayaBoutiqueDb.mssql.somee.com; Database=AbayaBoutiqueDb;User Id=asifalisabbir_SQLLogin_1;Password=o61shj57nu; Encrypt=False; MultipleActiveResultSets=True;";




//builder.Services.AddDbContext<BoutiqueDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<BoutiqueDbContext>(options =>
    options.UseSqlServer(currentConnectionString));

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

//builder.Services.AddSweetAlert2();
builder.Services.AddSweetAlert2(options => {
    options.Theme = SweetAlertTheme.Dark;
});

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

        //// This runs "dotnet ef database update" programmatically
        //if (context.Database.IsRelational())
        //{
        //    context.Database.Migrate();
        //}
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