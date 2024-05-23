using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesE_commerce.DBContect;
using MoviesE_commerce.Models;
using MoviesE_commerce.ViewModels;

using MoviesE_commerce.Models.ViewModels;

namespace MoviesE_commerce.Controllers
{

	public class AdminController : Controller
	{
		public IActionResult OneMovies(int id)
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
            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            ViewBag.Id = adminId;
            ViewBag.UserName = admin.FirstName;
            ViewBag.ImageURL = admin.ImageURL;

            return View(movie);
		}



		public IActionResult OneProducer(int id)
		{
            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            ViewBag.Id = adminId;
            ViewBag.UserName = admin.FirstName;
            ViewBag.ImageURL = admin.ImageURL;

            var producer = _db.Producers
				.Include(am => am.Movies)
				.FirstOrDefault(p => p.Id == id);

			if (producer == null)
			{
				return NotFound();
			}

			return View(producer);
		}



		public IActionResult OneActor(int id)
		{
            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            ViewBag.Id = adminId;
            ViewBag.UserName = admin.FirstName;
            ViewBag.ImageURL = admin.ImageURL;

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



		public IActionResult Index()
		{
			return View();
		}
		public IActionResult AdminDashboard()
		{
			//new
			

			int adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));

			var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
			//ViewBag.Id = admin;
			ViewBag.UserName = admin.FirstName;
			ViewBag.ImageURL = admin.ImageURL;

			ViewBag.AdminId = adminId;
			//prfile image
			//image 
			var user = _db.Users.FirstOrDefault(u => u.Id == adminId);
			ViewBag.UserId = adminId;

			var movies = _db.Movies.ToList(); // Retrieve all movies from the database
			return View(movies);
		}
		//Movie
		public int Id { get; set; }
		public string MovieName { get; set; }
		public string MovieDescription { get; set; }
		public int Price { get; set; }
		public string ImageURL { get; set; }
		public DateTime Date { get; set; }
		public TimeSpan Duration { get; set; }
		public MovieCategory MovieCategory { get; set; }
		public int ProducerMovieID { get; set; }

		public List<Actor> MovieActors;

		//Actor
		public string ActorName { get; set; }
		public string ActorBio { get; set; }
		public string ActorProfilePictureURL { get; set; }


		//Producer
		public string ProducerName { get; set; }
		public string ProducerBio { get; set; }
		public string ProducerProfilePictureURL { get; set; }

		//database
		private readonly MovieE_CommerceDbContext _db;

		public AdminController(MovieE_CommerceDbContext db)
		{
			_db = db;
		}

		//[HttpGet]
		//public IActionResult AddMovie()
		//{

		//	return View();
		//}

		//[HttpPost]
		//public IActionResult AddMovie(IFormCollection req)
		//{
		//	MovieName = req["MovieName"];
		//	string checkCategory = req["MovieCategory"]; // Moved the variable declaration here
		//	MovieDescription = req["MovieDescription"];
		//	Price = int.Parse(req["MoviePrice"]);
		//	ImageURL = req["MovieImage"];
		//	Date = DateTime.Parse(req["MovieDate"]);
		//	Duration = TimeSpan.Parse(req["MovieDuration"]);
		//	string ProducerMovie = req["ProducerMoview"];

		//	// Check if the movie already exists
		//	Movie existingMovie = _db.Movies.SingleOrDefault(movie => movie.Title == MovieName);
		//	if (existingMovie != null)
		//	{
		//		ViewData["Message"] = "This movie already exists";
		//		return View();
		//	}

		//	// Check if the provided category is valid
		//	bool isExistCategory = Enum.TryParse(checkCategory, out MovieCategory movieCategory);
		//	if (!isExistCategory)
		//	{
		//		ViewData["Message"] = "This category is not valid , our ctegory is Action,\r\nComedy,\r\nFamily,\r\nAdventure,\r\nFantasy,\r\nRomance,\r\nDocumentary";
		//		return View();
		//	}
		//	//add Producer
		//	Producer producer = _db.Producers.FirstOrDefault(producer => producer.Name == ProducerMovie);

		//	// Create and add the new movie
		//	Movie newMovie = new Movie
		//	{
		//		Title = MovieName,
		//		Description = MovieDescription,
		//		Price = Price,
		//		ImageURL = ImageURL,
		//		Date = Date,
		//		Duration = Duration,
		//		MovieCategory = movieCategory,
		//		ProducerId = producer.Id
		//	};
		//	_db.Movies.Add(newMovie);
		//	_db.SaveChanges();
		//	ViewData["Message"] = "Movie added successfully";

		//	return View();
		//}
		[HttpGet]
		public IActionResult AddProducer()
		{
            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            ViewBag.Id = adminId;
            ViewBag.UserName = admin.FirstName;
            ViewBag.ImageURL = admin.ImageURL;
            return View();

		}
		[HttpPost]

            
        //**************************
        public IActionResult AddProducer(IFormCollection req)
        {
            // Check if any field is empty
            if (string.IsNullOrEmpty(req["ProducerName"]) || string.IsNullOrEmpty(req["ProducerBio"]) || string.IsNullOrEmpty(req["ProfilePictureURL"]))
            {
                ViewData["Message"] = "All fields are required.";
                return View();
            }

            // Assign the values from the form collection
            string producerName = req["ProducerName"];
            string ProducerBio = req["ProducerBio"];
            string ProducerProfilePictureURL = req["ProfilePictureURL"];

            // Validate the length of the ActorName, ActorBio, and ProfilePictureURL
            if (producerName.Length > 30)
            {
                ViewData["Message"] = "Producer Name must be 30 characters or less.";
                return View();
            }

            if (ProducerBio.Length > 150)
            {
                ViewData["Message"] = "Producer Bio must be 150 characters or less.";
                return View();
            }

            if (ProducerProfilePictureURL.Length > 150)
            {
                ViewData["Message"] = "Profile Picture URL must be 150 characters or less.";
                return View();
            }

            // Check if ActorName contains only valid characters (letters and spaces)
            if (!IsValidActorName(producerName))
            {
                ViewData["Message"] = "Actor Name must contain only letters and spaces.";
                return View();
            }

            // Check if the actor already exists
            Producer? query = _db.Producers.SingleOrDefault(p=> p.Name == producerName);
            if (query != null)
            {
                ViewData["Message"] = "This Producer already exists.";
                return View();
            }

            // Create and add the new actor
            Producer newProducer = new Producer
            {
                Name = producerName,
                Bio = ProducerBio,
                ProfilePictureURL = ProducerProfilePictureURL
            };
            _db.Producers.Add(newProducer);
            _db.SaveChanges();
            ViewData["Message"] = "Producer added successfully";
            return RedirectToAction("Producers", "Admin");
        }

        //****************************************

        [HttpGet]
		public IActionResult AddActor()
		{
            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            ViewBag.Id = adminId;
            ViewBag.UserName = admin.FirstName;
            ViewBag.ImageURL = admin.ImageURL;
            return View();

		}
		[HttpPost]
        public IActionResult AddActor(IFormCollection req)
        {
            // Check if any field is empty
            if (string.IsNullOrEmpty(req["ActorName"]) || string.IsNullOrEmpty(req["ActorBio"]) || string.IsNullOrEmpty(req["ProfilePictureURL"]))
            {
                ViewData["Message"] = "All fields are required.";
                return View();
            }

            // Assign the values from the form collection
            string ActorName = req["ActorName"];
            string ActorBio = req["ActorBio"];
            string ActorProfilePictureURL = req["ProfilePictureURL"];

            // Validate the length of the ActorName, ActorBio, and ProfilePictureURL
            if (ActorName.Length > 30)
            {
                ViewData["Message"] = "Actor Name must be 30 characters or less.";
                return View();
            }

            if (ActorBio.Length > 150)
            {
                ViewData["Message"] = "Actor Bio must be 150 characters or less.";
                return View();
            }

            if (ActorProfilePictureURL.Length > 150)
            {
                ViewData["Message"] = "Profile Picture URL must be 150 characters or less.";
                return View();
            }

            // Check if ActorName contains only valid characters (letters and spaces)
            if (!IsValidActorName(ActorName))
            {
                ViewData["Message"] = "Actor Name must contain only letters and spaces.";
                return View();
            }

            // Check if the actor already exists
            Actor? query = _db.Actors.SingleOrDefault(actor => actor.Name == ActorName);
            if (query != null)
            {
                ViewData["Message"] = "This Actor already exists.";
                return View();
            }

            // Create and add the new actor
            Actor newActor = new Actor
            {
                Name = ActorName,
                Bio = ActorBio,
                ProfilePictureURL = ActorProfilePictureURL
            };
            _db.Actors.Add(newActor);
            _db.SaveChanges();
            ViewData["Message"] = "Actor added successfully";
            return RedirectToAction("Actors");
        }

        // Method to check if the actor name contains only letters and spaces
        private bool IsValidActorName(string actorName)
        {
            foreach (char c in actorName)
            {
                if (!char.IsLetter(c) && c != ' ')
                {
                    return false;
                }
            }
            return true;
        }


        public IActionResult Moviess()
		{
			int adminId ; // Initialize with default admin ID or retrieve from wherever it's stored

			// Retrieve the admin ID from the session or wherever it's stored
			
				adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
			//image 
			var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);

			// Set the admin ID in the ViewBag
			ViewBag.AdminId = adminId;

			//new
			//var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
			ViewBag.UserName = admin.FirstName;
			ViewBag.ImageURL = admin.ImageURL;
			//new

			var movies = _db.Movies.ToList(); 
			
			return View(movies);
		}


		public IActionResult Producers()
		{
			var producers = _db.Producers.
				GroupJoin(
                _db.Movies,
                producer => producer.Id,
                movie => movie.ProducerId,
                (producer, movies) => new { producer, movies }
            )
            .SelectMany(
                temp => temp.movies.DefaultIfEmpty(),
                (temp, movie) => new { temp.producer, movie }
            )
            .Where(temp => temp.movie == null)
            .Select(temp => temp.producer)
            .ToList(); // Retrieve all producers from the database
            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            ViewBag.Id = adminId;
            ViewBag.UserName = admin.FirstName;
            ViewBag.ImageURL = admin.ImageURL;

            return View(producers);
		}
		public IActionResult Actors()
		{
			var actors = _db.Actors.
                GroupJoin(
                _db.ActorMovies,
                actor => actor.Id,
                actorMovie => actorMovie.ActorId,
                (actor, actorMovies) => new { actor, actorMovies }
            )
            .SelectMany(
                temp => temp.actorMovies.DefaultIfEmpty(),
                (temp, actorMovie) => new { temp.actor, actorMovie }
            )
            .Where(temp => temp.actorMovie == null)
            .Select(temp => temp.actor)
            .ToList();

            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            ViewBag.Id = adminId;
            ViewBag.UserName = admin.FirstName;
            ViewBag.ImageURL = admin.ImageURL;
			
            return View(actors);
		}

		// GET: Producer/Delete/5
		public IActionResult DeleteProducer(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var producer = _db.Producers.Find(id);

			if (producer == null)
			{
				return NotFound();
			}

			return View(producer);
		}

		[HttpPost]
		public IActionResult DeleteProducer(int id)
		{
			var producer = _db.Producers.Find(id);
			if (producer == null)
			{
				return NotFound();
			}
			_db.Producers.Remove(producer);
			_db.SaveChanges();

			return RedirectToAction("Producers"); // Redirect to the producer list page
		}
		public IActionResult DeleteActor(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var actor = _db.Actors.Find(id);

			if (actor == null)
			{
				return NotFound();
			}

			return View(actor);
		}

		[HttpPost]
		public IActionResult DeleteActor(int id)
		{
			var actor = _db.Actors.Find(id);
			if (actor == null)
			{
				return NotFound();
			}
			_db.Actors.Remove(actor);
			_db.SaveChanges();

			return RedirectToAction("Actors"); // Redirect to the producer list page
		}

		//delete producer
		#region
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var producer = _db.Producers.Find(id);

			if (producer == null)
			{
				return NotFound();
			}

			return View(producer);
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			var producer = _db.Producers.Find(id);
			if (producer == null)
			{
				return NotFound();
			}
			_db.Producers.Remove(producer);
			_db.SaveChanges();

			return RedirectToAction("Producers"); // Redirect to the producer list page
		}
		#endregion
		//edit producer
		#region 
		
		public async Task<IActionResult> Edit(int? id)
		{
            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            ViewBag.Id = adminId;
            ViewBag.UserName = admin.FirstName;
            ViewBag.ImageURL = admin.ImageURL;

            if (id == null || _db.Producers == null)
			{
				return NotFound(); // Or handle appropriately
			}

			// Retrieve the producer from the database based on the ID
			var producer = await _db.Producers.FindAsync(id);
			if (producer == null)
			{
				return NotFound(); // Or handle appropriately
			}

			return View(producer); // Return the edit view with the producer data
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Bio,ProfilePictureURL")] Producer producer)
        {
            if (id != producer.Id)
            {
                return NotFound(); // Or handle appropriately
            }

            // Validate fields
            if (string.IsNullOrEmpty(producer.Name) || string.IsNullOrEmpty(producer.Bio) || string.IsNullOrEmpty(producer.ProfilePictureURL))
            {
                ViewData["Message"] = "All fields are required.";
                return View(producer);
            }

            if (producer.Name.Length > 30)
            {
                ViewData["Message"] = "Producer Name must be 30 characters or less.";
                return View(producer);
            }

            if (producer.Bio.Length > 150)
            {
                ViewData["Message"] = "Producer Bio must be 150 characters or less.";
                return View(producer);
            }

            if (producer.ProfilePictureURL.Length > 150)
            {
                ViewData["Message"] = "Profile Picture URL must be 150 characters or less.";
                return View(producer);
            }

            if (!IsValidActorName(producer.Name))
            {
                ViewData["Message"] = "Producer Name must contain only letters and spaces.";
                return View(producer);
            }

            // Check if the producer already exists (excluding the current producer being edited)
            var existingProducer = _db.Producers.SingleOrDefault(p => p.Name == producer.Name && p.Id != producer.Id);
            if (existingProducer != null)
            {
                ViewData["Message"] = "This Producer already exists.";
                return View(producer);
            }

            try
            {
                _db.Producers.Update(producer);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Producers)); // Redirect to the index page after successful edit
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while updating the producer.");
                return View(producer);
            }
        }

        #endregion

        [HttpGet]
		public async Task<IActionResult> EditMovie(int? id)
		{
			if (id == null || _db.Movies == null)
			{
				return NotFound();
			}

			var movie = await _db.Movies.FindAsync(id);
			if (movie == null)
			{
				return NotFound();
			}

			var actors = await _db.Actors.ToListAsync();


			var producer = await _db.Producers.ToListAsync();

			var viewModel = new EditMovieViewModel
			{
				Id = movie.Id,
				Title = movie.Title,
				Description = movie.Description,
				Price = (int)movie.Price,
				ImageURL = movie.ImageURL,
				Date = movie.Date,
				Duration = movie.Duration,
				MovieCategory = movie.MovieCategory,
				ProducerId = movie.ProducerId,
				Producers = _db.Producers.Select(p => new SelectListItem
				{
					Value = p.Id.ToString(),
					Text = p.Name
				}),
				Actors = _db.Actors.Select(a => new SelectListItem
				{
					Value = a.Id.ToString(),
					Text = a.Name
				})
			};
            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            ViewBag.Id = adminId;
            ViewBag.UserName = admin.FirstName;
            ViewBag.ImageURL = admin.ImageURL;

            return View(viewModel);
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditMovie(int id, EditMovieViewModel viewModel)
		{
			if (id != viewModel.Id)
			{
				return NotFound();
			}
			var movie = await _db.Movies.FindAsync(id);
			movie.Title = viewModel.Title;
			movie.Description = viewModel.Description;
			movie.Price = (int)viewModel.Price;
			movie.ImageURL = viewModel.ImageURL;
			movie.Date = viewModel.Date;
			movie.Duration = viewModel.Duration;
			movie.MovieCategory = viewModel.MovieCategory;
			movie.ProducerId = viewModel.ProducerId;


			_db.Movies.Update(movie);
			await _db.SaveChangesAsync();

			var existingActorMovies = _db.ActorMovies.Where(am => am.MovieId == id);
			_db.ActorMovies.RemoveRange(existingActorMovies);
			foreach (var actorId in viewModel.SelectedActorIds)
			{
				_db.ActorMovies.Add(new ActorMovie { MovieId = id, ActorId = actorId });
			}
			await _db.SaveChangesAsync();

            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            ViewBag.Id = adminId;
            ViewBag.UserName = admin.FirstName;
            ViewBag.ImageURL = admin.ImageURL;

            return RedirectToAction(nameof(Moviess));
		}

		private bool MovieExists(int id)
		{
			return _db.Movies.Any(e => e.Id == id);
		}



		public async Task<IActionResult> EditActor(int? id)
		{


			if (id == null || _db.Actors == null)
			{
				return NotFound(); // Or handle appropriately
			}

			// Retrieve the producer from the database based on the ID
			var actor = await _db.Actors.FindAsync(id);
			if (actor == null)
			{
				return NotFound(); // Or handle appropriately
			}

            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            ViewBag.Id = adminId;
            ViewBag.UserName = admin.FirstName;
            ViewBag.ImageURL = admin.ImageURL;

            return View(actor); // Return the edit view with the producer data
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActor(int id, [Bind("Id,Name,Bio,ProfilePictureURL")] Actor actor)
        {
            if (id != actor.Id)
            {
                return NotFound(); // Or handle appropriately
            }

            // Check if any field is empty
            if (string.IsNullOrEmpty(actor.Name) || string.IsNullOrEmpty(actor.Bio) || string.IsNullOrEmpty(actor.ProfilePictureURL))
            {
                ViewData["Message"] = "All fields are required.";
                return View(actor);
            }

            // Validate the length of the ActorName, ActorBio, and ProfilePictureURL
            if (actor.Name.Length > 30)
            {
                ViewData["Message"] = "Actor Name must be 30 characters or less.";
                return View(actor);
            }

            if (actor.Bio.Length > 150)
            {
                ViewData["Message"] = "Actor Bio must be 150 characters or less.";
                return View(actor);
            }

            if (actor.ProfilePictureURL.Length > 150)
            {
                ViewData["Message"] = "Profile Picture URL must be 150 characters or less.";
                return View(actor);
            }

            // Check if ActorName contains only valid characters (letters and spaces)
            if (!IsValidActorName(actor.Name))
            {
                ViewData["Message"] = "Actor Name must contain only letters and spaces.";
                return View(actor);
            }

            // Check if the actor name already exists (excluding the current actor being edited)
            var existingActor = _db.Actors.FirstOrDefault(a => a.Name == actor.Name && a.Id != actor.Id);
            if (existingActor != null)
            {
                ViewData["Message"] = "This Actor name already exists.";
                return View(actor);
            }

            try
            {
                // Update the actor in the database
                _db.Actors.Update(actor);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Actors)); // Redirect to the index page after successful edit
            }
            catch (Exception)
            {
                // Handle the error, maybe return to edit view with error message
                ModelState.AddModelError("", "An error occurred while updating the actor.");
                return View(actor);
            }
        }

        // Helper method to validate the actor name
 



        // GET: /Movies/AddMovie
        [HttpGet]
		public IActionResult AddMovieForm()
		{
			var viewModel = new MovieViewModel
			{
				Producers = _db.Producers.Select(p => new SelectListItem
				{
					Value = p.Id.ToString(),
					Text = p.Name
				}),
				Actors = _db.Actors.Select(a => new SelectListItem
				{
					Value = a.Id.ToString(),
					Text = a.Name
				})
			};
            int adminId = 0;
            adminId = Convert.ToInt32(HttpContext.Session.GetString("AdminId"));
            var admin = _db.Users.FirstOrDefault(u => u.Id == adminId);
            ViewBag.Id = adminId;
            ViewBag.UserName = admin.FirstName;
            ViewBag.ImageURL = admin.ImageURL;
            return View(viewModel);
		}

		// POST: /Movies/AddMovie
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddMovieForm(MovieViewModel viewModel)
		{

			var movie = new Movie
			{
				Title = viewModel.Title,
				Description = viewModel.Description,
				Price = (int)viewModel.Price,
				ImageURL = viewModel.ImageURL,
				Date = viewModel.Date,
				Duration = viewModel.Duration,
				MovieCategory = viewModel.MovieCategory,
				ProducerId = viewModel.ProducerId
			};

			_db.Movies.Add(movie);
			await _db.SaveChangesAsync();

			ActorMovie actormovie = new ActorMovie();

			// Map selected actor IDs to ActorMovies
			foreach (var actorId in viewModel.SelectedActorIds)
			{
				actormovie.ActorId = actorId;
				actormovie.MovieId = movie.Id;
				_db.ActorMovies.Add(actormovie);
				await _db.SaveChangesAsync();
				//movie.ActorMovies.Add(new ActorMovie { ActorId = actorId, MovieId = movie.Id });
			}



			// Redirect to a success page or another action
			return RedirectToAction("AdminDashboard", "Admin");

		}




		[HttpPost]
		public IActionResult SignOut()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");

		}
		[HttpGet]
		public IActionResult MoviesType(MovieCategory category)
		{

			var movies = _db.Movies.Where(m => m.MovieCategory == category).ToList();
			ViewData["Category"] = category;

			return View(movies);
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

				return RedirectToAction("NotFoundMovie", "UserProfile");
			}
			return RedirectToAction("OneMovies","Admin", movie);
		}
	}



	////////
	
}