using Microsoft.EntityFrameworkCore;
using AirMet.Models;
using AirMet.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PropertyDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:PropertyDbContextConnection"]);
});

builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.Run();
