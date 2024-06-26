﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FoodOrderingApp.Data;
using Microsoft.AspNetCore.Identity;
using DinkToPdf.Contracts;
using DinkToPdf;
using FoodOrderingApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FoodOrderingAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FoodOrderingAppContext") ?? throw new InvalidOperationException("Connection string 'FoodOrderingAppContext' not found.")));

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

var context = new CustomAssemblyLoadContext();
var libPath = Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox", "libwkhtmltox.dll");
context.LoadUnmanagedLibrary(libPath);

// Register DinkToPdf
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

// Add ProductRepository to the dependency injection container
builder.Services.AddTransient<DishRepository>();
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<OrderRepository>();
builder.Services.AddTransient<BasketRepository>();
builder.Services.AddTransient<PdfService>();
builder.Services.AddTransient<RazorViewToStringRenderer>();

builder.Services.AddSession(options =>
{
    // Set a short timeout for easy testing.
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Adjust as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // make the session cookie essential
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
