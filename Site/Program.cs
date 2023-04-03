using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models;
using System.Globalization;

// Create a new WebApplication builder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Get the connection string from the configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add the ApplicationDbContext to the services, using the connection string for SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add a filter that provides a developer exception page for database errors
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add the siteSettings.json file to the configuration, with optional reloading on change
builder.Configuration.AddJsonFile("siteSettings.json", true, reloadOnChange: true);

// Configure the SiteSettings options using the configuration
builder.Services.Configure<SiteSettings>(builder.Configuration);

// Configure localization options
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // Define supported cultures
    var supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("fi-FI")
        };

    // Add default identity with confirmed account requirement and EntityFramework stores
    options.DefaultRequestCulture = new RequestCulture("fi-FI"); // Set default culture to Finnish
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
// Add the AccountService as a scoped
builder.Services.AddScoped<Site.Utility.AccountService>();
// Add controllers and views to the application
builder.Services.AddControllersWithViews();
// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
// Use the MigrationsEndPoint middleware if in development mode
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // Use the exception handler middleware for production
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Serve static files from the wwwroot folder
app.UseStaticFiles();

// Add routing middleware
app.UseRouting();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Configure the default route for controllers and actions
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Add Razor pages routing
app.MapRazorPages();

// Create a new service scope using the app's services
// This ensures proper lifetime management and disposal of the scope
using (var scope = app.Services.CreateScope())
{
    // Retrieve an instance of the ApplicationDbContext from the scope's ServiceProvider
    // This ensures a properly scoped DbContext is used, following the service's lifetime
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Perform any pending database migrations to bring the database schema up-to-date
    // This ensures the application is using the latest schema and is particularly useful
    // when deploying updates to the database schema
    dbContext.Database.Migrate();
}

app.Run();