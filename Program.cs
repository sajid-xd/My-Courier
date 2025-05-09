using Microsoft.EntityFrameworkCore;
using mycourier.Models;
using Microsoft.Extensions.DependencyInjection;
using mycourier.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<mycourierContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("mycourierContext") ?? throw new InvalidOperationException("Connection string 'mycourierContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); //Add this here

var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();

builder.Services.AddDbContext<MycourierContext>(item =>
    item.UseSqlServer(config.GetConnectionString("hello")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession(); //Use session here (before endpoints)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
