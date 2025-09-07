using BlogSite.Data;
using BlogSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace BlogSite.Controllers
{

    public class HomeController : Controller
    {
        private readonly AppDbContext db;

        public HomeController(AppDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Post> Posts = db.Tbl_Post.ToList();

            return View(Posts);
        }

        [HttpGet]
        [Route("Home/Post/{slug}")]
        public IActionResult Post(string slug)
        {
            Post? post = db.Tbl_Post.FirstOrDefault(x => x.Slug == slug);
            return View(post);
        }

    }
}
