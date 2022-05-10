using Microsoft.EntityFrameworkCore;
using Shace.Logic.Accounts;
using Shace.Logic.Posts;
using Shace.Storage;
using Shace.Storage.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllersWithViews();
services.AddScoped<IAccountManager, AccountManager>();
services.AddScoped<IPostManager, PostManager>();

//Add DB Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<AccountContext>(options => options.UseSqlServer(connectionString));

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

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

app.UseAuthentication();    // àóòåíòèôèêàöèÿ
app.UseAuthorization();     // àâòîðèçàöèÿ

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
