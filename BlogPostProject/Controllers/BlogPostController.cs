using BlogPostProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BlogPostProject.Controllers
{
    public class BlogPostController : Controller
    {
        private readonly BlogContext _context;
      
        public BlogPostController(BlogContext context)
        {
            _context = context;
        }
        public IActionResult AllPosts()
        {
            var blogPosts = _context.BlogPosts.ToList();

            return View(blogPosts);
        }

        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]

        public IActionResult SubmitPost(BlogPost postfromform)
        {
            // Get the logged-in user's ID from the session
            var loggedInUserName = HttpContext.Session.GetString("UserName");
            if (loggedInUserName == null)
            {
                // If no user is logged in, redirect to login
                return RedirectToAction("Loginup", "User");
            }

            postfromform.AuthorName = loggedInUserName;
            postfromform.CreatedAt = DateTime.Now;
            _context.BlogPosts.Add(postfromform);
            _context.SaveChanges();

            return RedirectToAction("AllPosts");
      
        }
        public IActionResult MyPosts()
        {
            var loggedInUserName = HttpContext.Session.GetString("UserName");
            // Create SQL parameter to prevent SQL injection
            var userNameParam = new SqlParameter("@loggedInUserName", loggedInUserName);

          
           var posts = _context.BlogPosts.FromSqlRaw("SELECT * FROM BlogPosts WHERE AuthorName = @loggedInUserName ", userNameParam).ToList();

            return View(posts);
        }




        [HttpGet] 
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edited(int id, BlogPost blogPostfromWeb)
        {
            if (id != blogPostfromWeb.BlogPostId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var post = new BlogPost
                {
                    Title = blogPostfromWeb.Title,
                    Content = blogPostfromWeb.Content,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                    _context.Update(blogPostfromWeb);
                    _context.SaveChanges();
                
                
                return RedirectToAction(nameof(AllPosts));
            }

            return RedirectToAction("AllPosts");
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = _context.BlogPosts.FirstOrDefault(m => m.BlogPostId == id);

            if (blogPost == null)
            {
                return NotFound();
            }

            _context.BlogPosts.Remove(blogPost);
            _context.SaveChanges();

            return RedirectToAction("AllPosts");
        }
    }
}
