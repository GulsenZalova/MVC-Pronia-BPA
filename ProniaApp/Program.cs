using Microsoft.EntityFrameworkCore;
using ProniaApp.DAL; 
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(ops=>
   ops.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);
var app = builder.Build();

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
