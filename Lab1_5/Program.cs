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
builder.Services.AddScoped<IRepository<Room>, RoomRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ReservationRepository>();
builder.Services.AddScoped<RoomRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<RoomService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddHostedService<ReservationCleanupService>();

builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Add("/UI/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/UI/Views/Shared/{0}.cshtml");
    });

var app = builder.Build();

app.UseSession();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();