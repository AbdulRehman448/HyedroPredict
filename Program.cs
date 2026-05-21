using HydroPredict.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. REGISTER BLAZOR CORE SERVICES ---
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// --- 2. REGISTER LOCAL SQLITE DATABASE CONTEXT ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=HydroPredict.db"));

// --- 3. REGISTER CORE IDENTITY INFRASTRUCTURE ---
builder.Services.AddAuthentication();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
// --- 4. REGISTER CUSTOM DATA SERVICES ---
builder.Services.AddScoped<HydroPredict.Services.Interfaces.IAuthService, HydroPredict.Services.Implementations.AuthService>();
builder.Services.AddScoped<HydroPredict.Services.Interfaces.IBookingService, HydroPredict.Services.Implementations.BookingService>();
builder.Services.AddScoped<HydroPredict.Services.Interfaces.IDashboardService, HydroPredict.Services.Implementations.DashboardService>();

builder.Services.AddScoped<HydroPredict.Services.Interfaces.IAuthService, HydroPredict.Services.Implementations.AuthService>();
builder.Services.AddScoped<HydroPredict.Services.Interfaces.IBookingService, HydroPredict.Services.Implementations.BookingService>();
builder.Services.AddScoped<HydroPredict.Services.Interfaces.IDashboardService, HydroPredict.Services.Implementations.DashboardService>();
builder.Services.AddSingleton<HydroPredict.Services.Implementations.UiStateService>();


var app = builder.Build();


// --- 4. FIRST-RUN DATABASE AUTO-GENERATION TEST ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // This looks at your Models and physically creates the HydroPredict.db file with tables
    db.Database.EnsureCreated();
}

// --- 5. MIDDLEWARE PIPELINE ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<HydroPredict.Components.App>()
   .AddInteractiveServerRenderMode();

app.Run();