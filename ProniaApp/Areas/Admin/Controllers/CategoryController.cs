using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.DAL;
using ProniaApp.Models;

namespace ProniaApp.Admin.Controllers
{
    [Area("Admin")]
    
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context; 
        }
        public async Task<ActionResult> Index()
        {
            List<Category> categories=await _context.Categories.Include(c=>c.Products).ToListAsync();   
            return View(categories);
        }
    }
}