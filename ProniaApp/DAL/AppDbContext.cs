using Microsoft.EntityFrameworkCore;
using ProniaApp.Models;

namespace ProniaApp.DAL
{
     public class AppDbContext:DbContext
    {
       public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}

       public DbSet<Slide> Slides { get;set;} 
    }
}