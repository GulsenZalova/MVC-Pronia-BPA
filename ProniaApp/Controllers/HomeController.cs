using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.DAL;
using ProniaApp.Models;
using ProniaApp.ViewModels;

namespace ProniaApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context; 
        }
        public async Task<IActionResult> Index()

        {
            
           
             HomeVM homeVM = new HomeVM
             {
                //  Slides= slides.OrderBy(s=>s.Order).ToList()
                // Slides= slides.Take(2).ToList(),
                // Slides= slides.OrderBy(s=>s.Order).Take(2).ToList(),
                 Slides= await _context.Slides.OrderBy(s=>s.Order).Take(2).ToListAsync(),
                 Products=await  _context.Products.Include(p=>p.ProductImages).ToListAsync(),
             };
            

            return View(homeVM);
        
        }



        public ActionResult Details()
        {
            return View();  
        }
    }
}


// Adonet Dezavantajlari
// 1. cirkli koddur
// 2. kifayet qeder yuksek sql bilmelisen
// 3. sorguda sehv etme ehtimalimiz boyukdur ve compli time bunu gore bilmirik
// 4. SQL injection


//  ("Select * from Student" Truncate "")