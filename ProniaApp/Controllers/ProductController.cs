using Microsoft.AspNetCore.Mvc;

namespace ProniaApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductDetail()
        {
            return View();
        }
    }
}