using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProniaApp.DAL;
using ProniaApp.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(ops=>
   ops.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength=8;
    opt.Password.RequireNonAlphanumeric=false;

    opt.User.RequireUniqueEmail=true;

    opt.Lockout.AllowedForNewUsers=true;
    opt.Lockout.MaxFailedAccessAttempts=3;
    opt.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(3);
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();





var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllerRoute(
    name:"admin",
    pattern:"{area:exists}/{controller=home}/{action=index}/{id?}"
);
app.MapControllerRoute(
    name:"default",
    pattern:"{controller=home}/{action=index}/{id?}"
);

app.Run();
