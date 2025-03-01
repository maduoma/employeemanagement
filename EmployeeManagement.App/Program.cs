using EmployeeManagement.App.Data;
using EmployeeManagement.App.Models;
using EmployeeManagement.App.Profiles;
using EmployeeManagement.App.Repository;
using EmployeeManagement.App.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;  // Ensure this is present

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDbConnection")));

// Register employee repository and service dependencies
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Register the Contact repository and service
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

// Register AutoMapper with the EmployeeProfile
builder.Services.AddAutoMapper(typeof(EmployeeProfile));

// Register AutoMapper with the ContactProfile (and any other profiles)
builder.Services.AddAutoMapper(typeof(ContactProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Error/500");
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. 
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Authentication must come before Authorization.
app.UseAuthorization();

// Configure status code pages so that errors like 404 are handled by our ErrorController.
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



