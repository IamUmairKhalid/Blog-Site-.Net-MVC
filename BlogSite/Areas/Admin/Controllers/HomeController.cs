using BlogSite.Data;
using BlogSite.Models;
using BlogSite.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            if (HttpContext.Session.GetString("Valid") != null)
            {
                DisplayData();
                var posts = db.Tbl_Post.OrderByDescending(p => p.Date).ToList();
                if (posts == null || posts.Count == 0)
                {
                    return NotFound();
                }
                return View(posts);
            }
            return RedirectToAction("LoginView", "Home");
        }

        public IActionResult CreatePost()
        {
            if (HttpContext.Session.GetString("Valid") != null)
            {
                DisplayData();
                ViewBag.image = "true";
                return View();
            }
            return Redirect("/Admin/Home/LoginView/?ReturnUrl=/Admin/Home/CreatePost");
        }

        [HttpPost]
        public IActionResult CreatePost(PostVM post)
        {
            DisplayData();
            if (ModelState.IsValid && post.Image != null)
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
                return RedirectToAction("ViewPosts", "Home");
            }
            ViewBag.image = post.Image?.FileName.ToString();
            return View();
        }


        public IActionResult ViewPosts()
        {
            if(HttpContext.Session.GetString("Valid") != null)
            {
                DisplayData();
                var AllPosts = db.Tbl_Post;
                if (AllPosts == null)
                {
                    return NotFound();
                }
                return View(AllPosts);
            }
            return Redirect("/Admin/Home/LoginView?ReturnUrl=/Admin/Home/ViewPosts");
        }

        [HttpGet]
        [Route("Admin/Home/ViewDetails/{id}")]
        public IActionResult ViewDetails(int id)
        {
            if(HttpContext.Session.GetString("Valid") != null)
            {
                DisplayData();
                Post post = db.Tbl_Post.Find(id);
                if (post == null)
                {
                    return NotFound();
                }
                return View(post);
            }
            return Redirect("/Admin/Home/LoginView");
        }

        
        public IActionResult UpdatePost(int id)
        {
            if (HttpContext.Session.GetString("Valid") != null)
            {
                DisplayData();
                Post oldPost = db.Tbl_Post.Find(id);

                if (oldPost == null)
                {
                    return NotFound();
                }

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
            return Redirect("/Admin/Home/LoginView");
        }

        [HttpPost]
        public IActionResult UpdatePost(PostVM post, int id)
        {
            DisplayData();
            if (ModelState.IsValid)
            {
                Post newPost = new Post();
                string ImageName;

                newPost = db.Tbl_Post.Find(id);

                if(newPost == null)
                {
                    return NotFound();
                }

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
                return RedirectToAction("ViewPosts", "Home");
            }
            var existingData = db.Tbl_Post.Find(post.Id);
            ViewBag.CurrentImage = existingData?.Image;
            return View(post);
        }

        public IActionResult DeletePost(int id)
        {
            if(HttpContext.Session.GetString("Valid") != null)
            {
                var post = db.Tbl_Post.Find(id);
                if (post != null)
                {

                    string old_image = post.Image;
                    DeletePicture(old_image);

                    db.Tbl_Post.Remove(post);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ViewPosts", "Home");
        }


        public IActionResult ProfileView()
        {
            if (HttpContext.Session.GetString("Valid") != null)
            {
                DisplayData();
                Profile profile = db.Tbl_Profile.Find(HttpContext.Session.GetInt32("LoginId"));

                if (profile == null)
                {
                    return NotFound();
                }

                ProfileVM newprofile = new ProfileVM();

                newprofile.username = profile.username;
                newprofile.Password = profile.Password;
                newprofile.Name = profile.Name;
                newprofile.FatherName = profile.FatherName;
                newprofile.Id = profile.Id;
                newprofile.Bio = profile.Bio;
                ViewBag.ProfileImage = profile?.Image;
                
                return View(newprofile);
            }
            return Redirect("/Admin/Home/LoginView?ReturnUrl=/Admin/Home/ProfileView");
        }

        [HttpPost]
        public IActionResult ProfileView(ProfileVM profile)
        {
            DisplayData();
            if (ModelState.IsValid)
            {
                Profile newprofile = new Profile();
                string ImageName;

                newprofile = db.Tbl_Profile.Find(profile.Id);


                if (profile.Image != null)
                {
                    string old_image = newprofile.Image;
                    DeletePicture(old_image);

                    ImageName = profile.Id.ToString() + profile.Image.FileName.ToString();
                    var FolderPath = Path.Combine(env.WebRootPath, "Images");
                    var ImagePath = Path.Combine(FolderPath, ImageName);


                    var myFileStream = new FileStream(ImagePath, FileMode.Create);

                    profile.Image.CopyTo(myFileStream);
                    myFileStream.Close();

                    newprofile.Image = ImageName;
                }

                newprofile.username = profile.username;
                if(profile.Password != null)
                {
                    newprofile.Password = profile.Password;
                }
                newprofile.Name = profile.Name;
                newprofile.FatherName = profile.FatherName;
                newprofile.Id = profile.Id;
                newprofile.Bio = profile.Bio;
                ViewBag.ProfileImage = profile?.Image;

                db.Tbl_Profile.Update(newprofile);
                db.SaveChanges();

                return RedirectToAction("ProfileView", "Home");
            }
            var existingData = db.Tbl_Profile.Find(profile.Id);
            ViewBag.ProfileImage = existingData?.Image; 
            return View(profile);
        }

        public IActionResult LoginView()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginView(LoginVM user, string? returnUrl)
        {
            if(ModelState.IsValid)
            {
                var validuser = db.Tbl_Profile.Where(x => x.username == user.Username && x.Password == user.Password).FirstOrDefault();
                if(validuser != null)
                {
                    HttpContext.Session.SetInt32("LoginId", validuser.Id);
                    HttpContext.Session.SetString("Valid", "true");

                    if(returnUrl == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return  Redirect(returnUrl);
                    }
                }
                ViewBag.LoginFlag = "Invalid Username or Password";
                return View();
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home", new { area = "" });
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

        public void DisplayData()
        {
            ViewBag.Profile = db.Tbl_Profile.Where(x => x.Id == HttpContext.Session.GetInt32("LoginId")).AsNoTracking().FirstOrDefault();
        }
    }
}
