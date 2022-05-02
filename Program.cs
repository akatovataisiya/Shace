using Microsoft.EntityFrameworkCore;
using Shace.Logic.Accounts;
using Shace.Storage;
using Shace.Storage.Entities;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAccountManager, AccountManager>();

//Add DB Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<AccountContext>(options => options.UseSqlServer(connectionString));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
