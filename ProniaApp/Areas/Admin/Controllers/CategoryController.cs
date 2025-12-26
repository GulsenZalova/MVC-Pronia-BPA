using System.Reflection.Metadata;
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


       
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
          public async Task<ActionResult> Create(Category category)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            bool result =await  _context.Categories.AnyAsync(c=>c.Name.Trim()==category.Name.Trim());
            if (result)
            {
                ModelState.AddModelError("Name","category already exits");
                return View();
            }

            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            // return View("Index");
        }
      

      public async Task<ActionResult> Update(int? id)
        {
            // id null ve ya menfi olmamalidir
            // id-li melumati getirib gosterek
            if(id==null || id < 1)
            {
                return BadRequest();
            }

            Category category=await  _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);

             if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<ActionResult> Update(int? id,Category category)

        {
             if(id==null || id < 1)
            {
                return BadRequest();
            }
           
           Category exist=await  _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);

              if (category == null)
            {
                return NotFound();
            }
            // Model Statle bagli errorlari yazdirmaq lazimdir 
            if (!ModelState.IsValid)
            {
               return View();
            }

               bool result =await  _context.Categories.AnyAsync(c=>c.Name.Trim()==category.Name.Trim());
            if (result)
            {
                ModelState.AddModelError("Name","category already exits");
                return View();
            }


            exist.Name=category.Name;

            await _context.SaveChangesAsync();
           
            return RedirectToAction(nameof(Index));
        }


        public async Task<ActionResult> Delete(int? id)
        {
              if(id==null || id < 1)
            {
                return BadRequest();
            }

            Category category=await  _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);

              if (category == null)
            {
                return NotFound();
            }
            
             _context.Categories.Remove(category);
             await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        } 
    }
}