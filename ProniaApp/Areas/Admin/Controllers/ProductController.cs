using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.Admin.ViewModels;
using ProniaApp.DAL;
using ProniaApp.Models;

namespace ProniaApp.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize(Roles = "Admin,Moderator")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }





        public async Task<IActionResult> Index()
        {
            List<GetProductVM> productsVM = await
            _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true))
            .Select(p => new GetProductVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                Image = p.ProductImages[0].Image,
            })
            .ToListAsync();

            return View(productsVM);
        }

        public async Task<ActionResult> Create()
        {
            CreateProductVM createProductVM = new CreateProductVM()
            {
                Categories= await _context.Categories.ToListAsync(),
                Tags= await _context.Tags.ToListAsync()
            };
            return View(createProductVM);
        }
        

        [HttpPost]
             public async Task<IActionResult> Create(CreateProductVM createProductVM)
        {
            createProductVM.Categories = await _context.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(createProductVM);
            }
            
            Category category=await _context.Categories.FirstOrDefaultAsync(c=>c.Id==createProductVM.CategoryId);

            if (category is null)
            {
                ModelState.AddModelError(nameof(createProductVM.CategoryId), "bele bir category yoxdur");
                return View(createProductVM);
            }

            Product product = new ()
            {
                Name=createProductVM.Name,  
                Description=createProductVM.Description,
                Price=createProductVM.Price,
                SKU=createProductVM.SKU,
                CategoryId=createProductVM.CategoryId.Value,
                CreatedAt=DateTime.Now,
                IsDeleted=false
            };
            
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if(id==null || id < 1)
            {
                return BadRequest();
            }

            Product product = await _context.Products.FirstOrDefaultAsync(p=>p.Id==id);

            if (product is null)
            {
                return NotFound();
            }

             UpdateProductVM updateProductVM=new UpdateProductVM()
             {
                 Name=product.Name,
                 Price=product.Price,
                 SKU=product.SKU,   
                 Description=product.Description,
                 CategoryId=product.CategoryId,   
                 Categories= await _context.Categories.ToListAsync()
             };

            return View(updateProductVM);
        }


        [HttpPost]
        
        public async Task<ActionResult> Update(int? id, UpdateProductVM updateProductVM)
        {
              if(id==null || id < 1)
            {
                return BadRequest();
            }

            Product exists = await _context.Products.FirstOrDefaultAsync(p=>p.Id==id);

            if (exists is null)
            {
                return NotFound();
            }
            updateProductVM.Categories= await _context.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(updateProductVM);
            }

            bool result= await _context.Categories.AnyAsync(c=>c.Id==updateProductVM.CategoryId);

            if (!result)
            {
                ModelState.AddModelError(nameof(Category.Id),"bele category yoxdur");
            }
             
             exists.Name=updateProductVM.Name;
             exists.Price=updateProductVM.Price;
             exists.Description=updateProductVM.Description;
             exists.CategoryId=updateProductVM.CategoryId.Value;
             exists.SKU=updateProductVM.SKU;
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private static string Roles(string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }

}