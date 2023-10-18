using Microsoft.EntityFrameworkCore;
using AirMet.Models;
using AirMet.DAL;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PropertyDbContextConnection") ?? throw new InvalidOperationException("Connection string 'PropertyDbContextConnection' not found.");

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PropertyDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:PropertyDbContextConnection"]);
});

builder.Services.AddDefaultIdentity<IdentityUser> (options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<PropertyDbContext>();

builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();

builder.Services.AddRazorPages();
builder.Services.AddSession();

var loggerConfiguration = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File($"Logs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log");

loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) && e.Level == LogEventLevel.Information && e.MessageTemplate.Text.Contains("Executed DbCommand"));

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

app.UseStaticFiles();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseAuthentication();

app.MapDefaultControllerRoute();

app.MapRazorPages();

app.Run();
