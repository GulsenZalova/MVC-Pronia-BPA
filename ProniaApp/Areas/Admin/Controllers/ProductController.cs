using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.Admin.ViewModels;
using ProniaApp.DAL;
using ProniaApp.Models;

namespace ProniaApp.Admin.Controllers
{
    [Area("Admin")]

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
                Categories= await _context.Categories.ToListAsync()
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
    }

}