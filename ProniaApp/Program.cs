using Microsoft.EntityFrameworkCore;
using ProniaApp.DAL; 
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(ops=>
   ops.UseSqlServer("Server=localhost;Database=ProniaDbBPA201;User Id=sa;Password=future&forever&start&now_2025;TrustServerCertificate=True; Encrypt=False")
);
var app = builder.Build();

app.UseStaticFiles();
app.MapControllerRoute(
    name:"default",
    pattern:"{controller=home}/{action=index}"
);

app.Run();
