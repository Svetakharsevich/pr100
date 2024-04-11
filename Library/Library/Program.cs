using Library.Data;
using Library.Domain.Entities;
using Library.Domain.Services;
using Library.Infrastructure;
using Library.Middleware;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepository<User>, EFRepository<User>>();
builder.Services.AddScoped<IRepository<Role>, EFRepository<Role>>();
builder.Services.AddScoped<IRepository<Book>, EFRepository<Book>>();
builder.Services.AddScoped<IRepository<Category>, EFRepository<Category>>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBooksReader, BooksReader>();
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.ExpireTimeSpan = TimeSpan.FromHours(1);
        opt.Cookie.Name = "library_session";
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SameSite = SameSiteMode.Strict;
        opt.LoginPath = "/User/Login";
        opt.AccessDeniedPath = "/User/AccessDenied";
    });

builder.Services.AddDbContext<ELibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("local")));

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseBooksProtection();
app.UseStaticFiles();

app.MapControllerRoute("default", "{Controller=Books}/{Action=Index}");
app.Run();
