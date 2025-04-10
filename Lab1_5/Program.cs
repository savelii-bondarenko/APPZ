using Lab1_5.BusinessLogic.Services;
using Lab1_5.DataAccess;
using Lab1_5.DataAccess.Repositories;
using Lab1_5.Models.Entity;
using Lab1_5.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseLazyLoadingProxies()
        .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Add("/UI/Views/{1}/{0}.cshtml"); // Пошук у папці UI/Views/{Controller}/{Action}
        options.ViewLocationFormats.Add("/UI/Views/Shared/{0}.cshtml"); // Пошук у папці UI/Views/Shared
    });

builder.Services.AddHostedService<ReservationCleanupService>();

var app = builder.Build();

app.UseSession();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();