using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using MoviesE_commerce.DBContect;
using System.Data.Common;
using MoviesE_commerce.Models;


using Microsoft.AspNetCore.Http;

using Microsoft.IdentityModel.Tokens;
using MoviesE_commerce.Models.ViewModels;
using System.Text.Json;
using System.Security.Claims;


namespace MoviesE_commerce.Controllers
{

    public class RegisterController : Controller
    {

        Validation valid = new Validation();
        private object _accountService;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        private readonly MovieE_CommerceDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public RegisterController(MovieE_CommerceDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        public userRole userRole { get; set; }



        [HttpGet]
        public IActionResult SignUp()
        {

            if (HttpContext.Session.GetString("UserId") is not null)
            {
                return Redirect("/Register/SignIn");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(IFormCollection req)
        {
            ViewData["Message"] = "";
            FirstName = req["FirstName"];
            LastName = req["LastName"];
            Email = req["Email"];
            Password = req["Password"];
            PhoneNumber = req["PhoneNumber"];

      
            var profilePicture = req.Files["ImageURL"];
       
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
                _db.SaveChanges();

            }
            var imageURL = "/images/" + fileName;

            User? query = _db.Users.SingleOrDefault(user => user.Email == Email);

            bool error = true;
            if (query != null)
            {
                ViewData["Message"] = "this account is already exist";
            }
            else
            {
                if (!valid.isValidName(FirstName))
                {
                    ViewData["Message"] += "Inavlid First Name\n";
                    error = false;

                }
                if (!valid.isValidName(LastName))
                {
                    ViewData["Message"] += "Inavlid Last Name\n";
                    error = false;

                }
                if (!valid.isValidEmail(Email))
                {
                    ViewData["Message"] += "Inavlid Email\n";
                    error = false;

                }
                if (!valid.isValidPassword(Password))

                {
                    ViewData["Message"] += "Inavlid Password : it should contains at least four capital letters and is at least 8 characters long, no spaces\n";
                    error = false;
                }
                if (!valid.isValidPhone(PhoneNumber))
                {
                    ViewData["Message"] += "Inavlid Phone number\n";
                    error = false;

                }
                if (error)
                {
                    User newUser = new User()
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Email = Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(Password),
                        PhoneNumber = PhoneNumber,
                        userRole = userRole.User,
                        ImageURL = imageURL
                    };
                    _db.Users.Add(newUser);
                    _db.SaveChanges();
                    HttpContext.Session.SetString("UserId", newUser.Id.ToString());
                    HttpContext.Session.SetString("UserRole", newUser.userRole.ToString());

                    //Response.Redirect("/SignIn", false, true);
                    ViewData["Message"] = "pass";
                   // return RedirectToAction("ViewProfile", "UserProfile");
                    return RedirectToAction("UserDashboard", "UserProfile");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Home", new { id = HttpContext.Session.GetString("UserId") });
            }
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(string Email, string Password)
        {
            // Checking the existence of the account with the correct password
            var query = _db.Users.SingleOrDefault(user => user.Email == Email);
            try
            {
				if (query == null)
				{
					TempData["Message"] = "This email doesn't exist";
					return RedirectToAction("SignIn");
				}
				if (!BCrypt.Net.BCrypt.Verify(Password, query.Password))
				{
					TempData["Message"] = "Incorrect password. Please try again.";
					return RedirectToAction("SignIn");
				}
				else if (query.userRole == userRole.User)
                {
                    HttpContext.Session.SetString("UserId", query.Id.ToString());
                    HttpContext.Session.SetString("UserRole", query.userRole.ToString());
                    return RedirectToAction("UserDashboard", "UserProfile", new { id = query.Id });
                }
                else
                {
                    HttpContext.Session.SetString("AdminId", query.Id.ToString());
                    HttpContext.Session.SetString("UserRole", query.userRole.ToString());
                    return RedirectToAction("AdminDashboard", "Admin", new { id = query.Id });
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Invalid email or password. Please try again.";
                return RedirectToAction("SignIn");
            }
        }


        //[HttpGet]
        //public IActionResult LogIn()
        //{
          
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult LogIn(LoginViewModel model)
        //{
        //    LoginViewModel vm = _accountService.LogIn(model);
        //    if(vm != null) 
        //    {
        //        string sessionObj = JsonSerializer.Serialize(model);
        //        HttpContext.Session.SetString("LoginDetails", sessionObj);
        //        var claims = new List<Claim>()
        //        {
        //            new Claim(ClaimTypes,Name,model.Email)
        //        };
        //    }
           
        //    return View(model);
        //}
    }
}
