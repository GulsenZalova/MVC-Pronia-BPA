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
        public IActionResult Index()

        {
       

            // _context.Slides.AddRange(slides);   
            // _context.SaveChanges();
           
             HomeVM homeVM = new HomeVM
             {
                //  Slides= slides.OrderBy(s=>s.Order).ToList()
                // Slides= slides.Take(2).ToList(),
                // Slides= slides.OrderBy(s=>s.Order).Take(2).ToList(),
                 Slides= _context.Slides.OrderBy(s=>s.Order).Take(2).ToList(),
                 Products= _context.Products.Include(p=>p.Category).ToList(),
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