using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.Admin.ViewModels;
using ProniaApp.DAL;
using ProniaApp.Models;
using ProniaApp.Utilities.Enums;
using ProniaApp.Utilities.Extensions;

namespace ProniaApp.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context; 
            _env = env;
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

        public ActionResult Test()
        { 
            // c41983a6-b0f3-4e21-96ca-a3cf4b5156femyflowers
           return Content(Guid.NewGuid().ToString());
        }

        [HttpPost]  
         public  async Task<ActionResult> Create(CreateSlideVM createSlideVM)

        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // slide.Photo.ValidateType("image/")
            if (!createSlideVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo","bu file formata uygun deyil");
                return View();
            }

            if (createSlideVM.Photo.ValidateSize(FileSize.KB, 15 ))
            {
                 ModelState.AddModelError("Photo","bu file olcusu 2mg-dan coxdur");
                 return View();
            }

            // file yolunu dinamik olaraq goturmuruk
            // sekilleri eyni adla serverde saxlayiriq

            // string filename=String.Concat(Guid.NewGuid().ToString(),slide.Photo.FileName);
            // string path= Path.Combine(_env.WebRootPath,"assets","images","website-images",filename);
            // FileStream fileStream=new(path,FileMode.Create);
            
            // await slide.Photo.CopyToAsync(fileStream);
            // fileStream.Close();
           


          
           Slide slide = new Slide()
           {
               Title=createSlideVM.Title,
               Subtitle=createSlideVM.Subtitle,
               Description=createSlideVM.Description,
               Order=createSlideVM.Order,
               Image= await createSlideVM.Photo?.CreateFile(_env.WebRootPath,"assets","images","website-images"),
               CreatedAt=DateTime.Now,
               IsDeleted=false,
           };
      
           await _context.Slides.AddAsync(slide);
           await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        //    return Content(slide.Photo.FileName + " " +  slide.Photo.ContentType + "" + slide.Photo.Length);
        }


         public async Task<ActionResult> Delete(int? id)
        {
            if(id==null || id < 1) return BadRequest();
            

            Slide slide=await _context.Slides.FirstOrDefaultAsync(s => s.Id==id);

            if(slide==null) return NotFound();
            


            // System.IO.File.Delete(Path.Combine(_env.WebRootPath,"assets","images","website-images",slide.Image));

             slide.Image.DeleteFile(_env.WebRootPath,"assets","images","website-images");
            _context.Slides.Remove(slide);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



        public async Task<ActionResult> Update(int? id)
        {
            if(id==null || id < 1) return BadRequest();
            

            Slide slide=await _context.Slides.FirstOrDefaultAsync(s => s.Id==id);

            if(slide==null) return NotFound();
            
            UpdateSlideVM updateSlideVM =new  UpdateSlideVM(){
                Title=slide.Title,
                Description=slide.Description,
                Order=slide.Order,
                Subtitle=slide.Subtitle,
                Image=slide.Image,
            };

            return View(updateSlideVM);
        } 

        [HttpPost]
        public async Task<ActionResult> Update(int? id,UpdateSlideVM updateSlideVM)
        {
            if(id==null || id < 1) return BadRequest();
            

            Slide slide=await _context.Slides.FirstOrDefaultAsync(s => s.Id==id);

            if(slide==null) return NotFound();


            if (!ModelState.IsValid)
            {
                return View(updateSlideVM);
            }

            if(updateSlideVM.Photo is not null)
            {
                if (!updateSlideVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateSlideVM.Photo), "invalid type");
                    return View(updateSlideVM);
                }

                if (updateSlideVM.Photo.ValidateSize(FileSize.KB, 15))
                {
                      ModelState.AddModelError(nameof(UpdateSlideVM.Photo), "invalid size");
                    return View(updateSlideVM);
                }

                string filename= await updateSlideVM.Photo.CreateFile(_env.WebRootPath,"assets","images","website-images");
                slide.Image.DeleteFile(_env.WebRootPath,"assets","images","website-images");
                slide.Image=filename;
            }
            slide.Title=updateSlideVM.Title;
            slide.Description=updateSlideVM.Description;    
            slide.Subtitle=updateSlideVM.Subtitle;
            slide.Order=updateSlideVM.Order; 

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }




}