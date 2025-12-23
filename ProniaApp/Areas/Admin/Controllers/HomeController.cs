using Microsoft.AspNetCore.Mvc;

namespace ProniaApp.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}