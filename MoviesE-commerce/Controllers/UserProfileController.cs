using Microsoft.AspNetCore.Mvc;
using MoviesE_commerce.DBContect;
using System.Data.Common;
using MoviesE_commerce.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http;

using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using MoviesE_commerce.filters;

namespace MoviesE_commerce.Controllers
{
	//[RoleAuthorize(1)]
	public class UserProfileController : Controller
	{

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult UserDashboard()
		{
			int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

			ViewBag.UserId = userId;
			//prfile image
			//image 
			var user = _db.Users.FirstOrDefault(u => u.Id == userId);
			ViewBag.UserId = userId;

			//int id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
			//var user = _db.Users.FirstOrDefault(u => u.Id == id);
			//ViewBag.Id = id;
			ViewBag.UserName = user.FirstName;
			ViewBag.ImageURL = user.ImageURL;

			var movies = _db.Movies.ToList();
			return View(movies);
		}
		
		Validation valid = new Validation();
        private readonly MovieE_CommerceDbContext _db;
        private readonly IWebHostEnvironment _environment;
        public UserProfileController(MovieE_CommerceDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }
        public string newFirstName { get; set; }
		public string newLastName { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }


        public IActionResult ViewProfile()
		{
			int id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

			var user = _db.Users.SingleOrDefault(user => user.Id == id);
			if (user == null)
			{
				return RedirectToAction("SignIn", "Register");

			}
			return View(user);
		}
		[HttpPost]
        public async Task<IActionResult> UpdatePhoto(IFormCollection req)
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            var userPhoto = _db.Users.SingleOrDefault(user => user.Id == id);
            var newImage = req.Files["Img"]; 

            if (newImage != null && newImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newImage.CopyToAsync(stream);
                }

                var imageURL = "/images/" + fileName;

                if (userPhoto.ImageURL != null)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", userPhoto.ImageURL.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                userPhoto.ImageURL = imageURL;
                _db.SaveChanges(); // Save changes after updating the image URL
            }

            return RedirectToAction("ViewProfile","UserProfile");
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePhotoForAdmin(IFormCollection req)
        {
            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            var newImage = req.Files["Img"];

            if (newImage != null && newImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newImage.CopyToAsync(stream);
                }

                var imageURL = "/images/" + fileName;

                if (admin.ImageURL != null)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", admin.ImageURL.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                admin.ImageURL = imageURL;
                _db.SaveChanges(); // Save changes after updating the image URL
            }

            return RedirectToAction("adminProfile", "UserProfile");
        }

        [HttpGet]
		public IActionResult Edit()
		{
			if (HttpContext.Session.GetString("UserId") is null)
			{
				return RedirectToAction("SignIn", "Register");
			}

			return View();


		}
		[HttpPost]
		public IActionResult Edit(IFormCollection req)
		{
            
            int id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            var edituser = _db.Users.SingleOrDefault(user => user.Id == id);

            newFirstName = req["FirstName"];
			newLastName = req["LastName"];
			oldPassword = req["oldPassword"];
            newPassword = req["newPassword"];
           
            bool error = true;



			if (!valid.isValidName(newFirstName))
			{
				ViewData["Message"] += "Inavlid First Name\n";
				error = false;

			}
			if (!valid.isValidName(newLastName))
			{
				ViewData["Message"] += "Inavlid Last Name\n";
                error = false;

			}
			if (!valid.isValidPassword(newPassword))
			{
				TempData["Message"] += "Inavlid Password : it should contains at least four capital letters and is at least 8 characters long, no spaces\n";
				error = false;
				return View();
			}
			if (edituser == null || !BCrypt.Net.BCrypt.Verify(oldPassword, edituser.Password))
			{
				TempData["Message"] += "Old Password is not correct\n";
				error = false;
			
			}

            if (error)
			{

				if (edituser == null)
				{
					ViewData["Message"] += $"Null Value, {id}\n";
					return View();
				}
				edituser.FirstName = newFirstName;
				edituser.LastName = newLastName;
				edituser.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
				_db.SaveChanges();
				ViewData["Message"] = "Changes Saved";
				return RedirectToAction("ViewProfile", "UserProfile");
			}
			return View();
			
		}
		[HttpGet]
		public IActionResult DeleteUser(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = _db.Users.Find(id);

			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}
		[HttpPost]
		public IActionResult DeleteUser(int id)
		{
			var user = _db.Users.Find(id);
			if (user == null)
			{
				return NotFound();
			}
			_db.Users.Remove(user);
			_db.SaveChanges();
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home"); // Redirect to the producer list page
		}

		[HttpPost]
		public IActionResult SignOut()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");

		}
		public IActionResult Movies()
		{
			int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
			
			ViewBag.UserId = userId;
			
			var user = _db.Users.FirstOrDefault(u => u.Id == userId);
			ViewBag.Id = userId;
			ViewBag.UserName = user.FirstName;
			ViewBag.ImageURL = user.ImageURL;


			var movies = _db.Movies.ToList();
			return View(movies);
		}
		public IActionResult OneMovies(int id)
		{
			//new
			int Id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

			var user = _db.Users.FirstOrDefault(u => u.Id == Id);
			if (user == null)
			{
				return RedirectToAction("HomeOneMovie");
			}
			ViewBag.Id = Id;
			ViewBag.UserName = user.FirstName;
			ViewBag.ImageURL = user.ImageURL;


			var movie = _db.Movies
				.Include(m => m.Producer)
				.Include(m => m.ActorMovies)
					.ThenInclude(am => am.Actor)
				.FirstOrDefault(m => m.Id == id);

			if (movie == null)
			{
				return NotFound();
			}

			return View(movie);
		}
		public IActionResult HomeOneMovie(int id)
		{

			var movie = _db.Movies
				.Include(m => m.Producer)
				.Include(m => m.ActorMovies)
					.ThenInclude(am => am.Actor)
				.FirstOrDefault(m => m.Id == id);

			if (movie == null)
			{
				return NotFound();
			}

			return View("OneMovies",movie);
		}
		public IActionResult Producer(int id)
		{
			var producer = _db.Producers
				.Include(am => am.Movies)
				.FirstOrDefault(p => p.Id == id);

			if (producer == null)
			{
				return NotFound(); 
			}
			return View(producer);
		}

		public IActionResult Actor(int id)
		{
			
			var actor = _db.Actors
				.Include(am => am.ActorMovies) 
					.ThenInclude(am => am.Movie) 
				.FirstOrDefault(a => a.Id == id);

			if (actor == null)
			{
				return NotFound(); 
			}

			return View(actor);
		}
		[HttpGet]
        public IActionResult MoviesType(MovieCategory category)
        {
            
            var movies = _db.Movies.Where(m => m.MovieCategory == category).ToList();
			ViewData["Category"] = category;

			return View(movies);
        }
		public IActionResult NotFoundMovie()
		{
			return View();
		}
		[HttpGet]
		public IActionResult Search()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Search(string searchTerm)
		{
			var movie = _db.Movies.FirstOrDefault(m => m.Title.Contains(searchTerm));

			if (movie == null)
			{
				
				return View("NotFoundMovie");
			}
			return RedirectToAction("OneMovies","UserProfile", movie);
		}

		public IActionResult adminProfile()
		{
			int adminId = 0;
			if (HttpContext.Session.GetString("AdminId") != null)
			{
				adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
				var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
				if (admin == null)
				{
					return RedirectToAction("SignIn", "Register");

				}
				ViewBag.Id = admin;
				ViewBag.UserName = admin.FirstName;
				ViewBag.ImageURL = admin.ImageURL;
				return View(admin);

			}


			return RedirectToAction("SignIn", "Register");

		}

	}
}
