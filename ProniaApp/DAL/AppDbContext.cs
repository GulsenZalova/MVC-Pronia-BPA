using Microsoft.EntityFrameworkCore;
using ProniaApp.Models;

namespace ProniaApp.DAL
{
     public class AppDbContext:DbContext
    {
       public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}

       public DbSet<Slide> Slides { get;set;} 
       public DbSet<Product> Products { get;set;}

       public DbSet<Category> Categories { get;set;}
       public DbSet<ProductImage> ProductsImages { get;set;}
    }
}