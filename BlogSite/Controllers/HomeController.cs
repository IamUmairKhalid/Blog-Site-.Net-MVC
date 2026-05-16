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

        
        public IActionResult Index(string? searchquery)
        {
            LayoutData();
            if(searchquery != null)
            {
                IEnumerable<Post> SearchedPost = db.Tbl_Post.OrderDescending().Where(x => x.Content.Contains(searchquery));
                return View(SearchedPost);
            }
            IEnumerable<Post> Posts = db.Tbl_Post.OrderDescending().ToList();
            return View(Posts);
        }

        
        [Route("Home/Post/{slug}")]
        public IActionResult Post(string slug)
        {
            LayoutData();

            Post? post = db.Tbl_Post.FirstOrDefault(x => x.Slug == slug);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        public void LayoutData()
        {
            ViewBag.posts = db.Tbl_Post;
            ViewBag.profile = db.Tbl_Profile.FirstOrDefault();
        }

        [Route("Home/HandleError/{code}")]
        public IActionResult HandleError(int code)
        {
            if (code == 404)
            {
                return View("NotFound");
            }
            return View("Error");
        }
    }
}
