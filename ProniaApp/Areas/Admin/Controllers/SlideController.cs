using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.DAL;
using ProniaApp.Models;

namespace ProniaApp.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        public SlideController(AppDbContext context)
        {
            _context = context; 
        }
        public async Task<ActionResult> Index()
        {
            List<Slide> slides=await _context.Slides.ToListAsync();   
            return View(slides);
        }

        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]  
         public  async Task<ActionResult> Create(Slide slide)

        {
         

            if (!slide.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo","bu file formata uygun deyil");
                return View();
            }

            if (slide.Photo.Length > 2 * 1024 * 1024)
            {
                 ModelState.AddModelError("Photo","bu file olcusu 2mg-dan coxdur");
                 return View();
            }
            string path="/Users/gulshanzalova/Desktop/MVCProjects-BPA/ProniaApp/wwwroot/assets/images/website-images/" + slide.Photo.FileName;
            FileStream fileStream=new(path,FileMode.Create);
            
            await slide.Photo.CopyToAsync(fileStream);
            fileStream.Close();

            slide.Image=slide.Photo.FileName;

               if (!ModelState.IsValid)
            {
                return View();
            }

           await _context.Slides.AddAsync(slide);
           await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        //    return Content(slide.Photo.FileName + " " +  slide.Photo.ContentType + "" + slide.Photo.Length);
        }
    }
}