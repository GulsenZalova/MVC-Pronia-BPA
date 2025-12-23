using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.DAL;
using ProniaApp.Models;
using ProniaApp.ViewModels;

namespace ProniaApp.Controllers
{
    public class ShopController : Controller
    {
         private readonly AppDbContext _context;
        public ShopController(AppDbContext context)
        {
            _context = context; 
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int? id)
        {
            if(id==null || id < 1)
            {
                return BadRequest();
            }
            Product product= _context.Products.Include(p=>p.ProductImages).Include(p=>p.Category).FirstOrDefault(p => p.Id==id);

            if (product == null)
            {
                return NotFound();
            }
            DetailVM detailVM= new DetailVM
            {
                Product= product,
                RelatedProducts=_context.Products.Where(p=>p.CategoryId==product.CategoryId&&p.Id!=id).ToList()
            };
            return View(detailVM);
        }
    }
}