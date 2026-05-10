using BlogSite.Data;
using BlogSite.Models;
using BlogSite.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BlogSite.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment env;

        public HomeController(AppDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePost(PostVM post)
        {
            if (ModelState.IsValid)
            {
                Post newPost = new Post();

                string ImageName = post.Image.FileName.ToString();

                var FolderPath = Path.Combine(env.WebRootPath, "Images");
                var ImagePath = Path.Combine(FolderPath, ImageName);


                var myFileStream = new FileStream(ImagePath, FileMode.Create);

                post.Image.CopyTo(myFileStream);
                myFileStream.Close();

                newPost.Image = ImageName;
                newPost.Slug = post.Slug;
                newPost.Title = post.Title;
                newPost.SubTitle = post.SubTitle;
                newPost.Content = post.Content;
                newPost.Date = post.Date;

                db.Tbl_Post.Add(newPost);
                db.SaveChanges();
            }

            return RedirectToAction("ViewPosts", "Home");
        }


        public IActionResult ViewPosts()
        {
            var AllPosts = db.Tbl_Post;

            return View(AllPosts);
        }

        [HttpGet]
        [Route("Admin/Home/ViewDetails/{id}")]
        public IActionResult ViewDetails(int id)
        {
            Post post = db.Tbl_Post.Find(id);

            return View(post);
        }

        
        public IActionResult UpdatePost(int id)
        {
            Post oldPost = db.Tbl_Post.Find(id);

            PostVM post = new PostVM();

            post.Id = oldPost.Id;
            post.Title = oldPost.Title;
            post.SubTitle = oldPost.SubTitle;
            post.Content = oldPost.Content;
            post.Date = oldPost.Date;
            post.Slug = oldPost.Slug;

            ViewBag.CurrentImage = oldPost.Image;


            return View(post);
        }

        [HttpPost]
        public IActionResult UpdatePost(PostVM post, int id)
        {
            if (ModelState.IsValid)
            {
                Post newPost = new Post();
                string ImageName;

                newPost = db.Tbl_Post.Find(id);


                if (post.Image != null)
                {
                    string old_image = newPost.Image;
                    DeletePicture(old_image);

                    ImageName = id.ToString()+post.Image.FileName.ToString();
                    var FolderPath = Path.Combine(env.WebRootPath, "Images");
                    var ImagePath = Path.Combine(FolderPath, ImageName);


                    var myFileStream = new FileStream(ImagePath, FileMode.Create);

                    post.Image.CopyTo(myFileStream);
                    myFileStream.Close();

                    newPost.Image = ImageName;
                }
                newPost.Slug = post.Slug;
                newPost.Title = post.Title;
                newPost.SubTitle = post.SubTitle;
                newPost.Content = post.Content;
                newPost.Date = post.Date;

                db.Tbl_Post.Update(newPost);
                db.SaveChanges();
            }

            return RedirectToAction("ViewPosts", "Home");
        }

        public IActionResult DeletePost(int id)
        {
            var post = db.Tbl_Post.Find(id);
            if (post != null)
            {

                string old_image = post.Image;
                DeletePicture(old_image);

                db.Tbl_Post.Remove(post);
                db.SaveChanges();
            }
            return RedirectToAction("ViewPosts", "Home", "Admin");
        }

        public void DeletePicture(string old_pic)
        {
            var FolderPath = Path.Combine(env.WebRootPath, "images");
            var ImagePath = Path.Combine(FolderPath, old_pic);

            var flag = System.IO.File.Exists(ImagePath);

            if (flag)
            {
                System.IO.File.Delete(ImagePath);
            }
        }

    }
}
