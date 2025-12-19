using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreatePost()
        {
            return View();
        }

    }
}
