using AdSetSolution.Application.Interfaces;
using AdSetSolution.Application.Services;
using AdSetSolution.Domain.Interfaces;
using AdSetSolution.Infrastructure.Data;
using AdSetSolution.Infrastructure.Mapping;
using AdSetSolution.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(context =>
    context.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IVehicleImgService, VehicleImgService>();
builder.Services.AddScoped<IVehiclePackageService, VehiclePackageService>();

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehiclePackageRepository, VehiclePackageRepository>();
builder.Services.AddScoped<IVehicleImgRepository, VehicleImgRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();

builder.Services.AddRazorPages();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
