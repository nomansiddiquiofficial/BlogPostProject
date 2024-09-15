using BlogPostProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogPostProject.Controllers
{
    public class UserController : Controller

    {    private readonly BlogContext _context;

        public UserController(BlogContext context) {
            _context = context;
        }


        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signedup(User userfromform)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.UserName == userfromform.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return RedirectToAction("Signup"); 
                }

                var newUser = new User
                {
                    UserName = userfromform.UserName,
                    Password = userfromform.Password,
                    Role = userfromform.Role,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                    
                _context.Users.Add(newUser);
                _context.SaveChanges();

                return RedirectToAction("Loginup");
            }
           

            return RedirectToAction("AllPosts");
        }

        [HttpGet]
        public IActionResult Loginup()
        {
            return View();
        }


        [HttpPost]
        public IActionResult LogedIn(User userfromform)
        {
          
                // Check if the user exists and password matches
                var user = _context.Users.FirstOrDefault(u => u.UserName == userfromform.UserName && u.Password == userfromform.Password);


            // Set user session
                     HttpContext.Session.SetInt32("UserId", user.UserId); // Store UserId in the session
                     HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("Role", user.Role);

           
            _context.SaveChanges();
            return RedirectToAction("CreatePost", "BlogPost");
          
        }


        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();
            return RedirectToAction("Loginup");
        }
    }




}

