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
		public IActionResult AddProducer(IFormCollection req)
		{

			ProducerName = req["ProducerName"];
			ProducerBio = req["ProducerBio"];
			ProducerProfilePictureURL = req["ProfilePictureURL"];
			Producer? query = _db.Producers.SingleOrDefault(producer => producer.Name == ProducerName);

			if (query != null)
			{
				ViewData["Message"] = "this Producer is already exist";
				return View();
			}

			Producer newProducer = new Producer
			{
				Name = ProducerName,
				Bio = ProducerBio,
				ProfilePictureURL = ProducerProfilePictureURL
			};

			_db.Producers.Add(newProducer);
			_db.SaveChanges();
			ViewData["Message"] = "Producer added successfully";

           

            return View();
		}
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
			ActorName = req["ActorName"];
			ActorBio = req["ActorBio"];
			ActorProfilePictureURL = req["ProfilePictureURL"];

			Actor newActor = new Actor
			{
				Name = ActorName,
				Bio = ActorBio,
				ProfilePictureURL = ActorProfilePictureURL
			};
			_db.Actors.Add(newActor);
			_db.SaveChanges();
			ViewData["Message"] = "Actor added successfully";
			return View();
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

			try
			{
				// Update the producer in the database
				_db.Producers.Update(producer);
				await _db.SaveChangesAsync(); // This line is missing in your provided code
				return RedirectToAction(nameof(Producers)); // Redirect to the index page after successful edit
			}
			catch (Exception)
			{
				// Handle the error, maybe return to edit view with error message
				ModelState.AddModelError("", "An error occurred while updating the producer.");
				return View(producer);
			}


			// If model state is not valid, return to the edit view with the model

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

			try
			{
				// Update the actor in the database
				_db.Actors.Update(actor);
				await _db.SaveChangesAsync(); // This line is missing in your provided code
				return RedirectToAction(nameof(Actors)); // Redirect to the index page after successful edit
			}
			catch (Exception)
			{
				// Handle the error, maybe return to edit view with error message
				ModelState.AddModelError("", "An error occurred while updating the producer.");
				return View(actor);
			}


			// If model state is not valid, return to the edit view with the model

		}


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

				return View("NotFoundMovie");
			}
			return RedirectToAction("OneMovies", movie);
		}
	}



	////////
	
}