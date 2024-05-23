using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using MoviesE_commerce.Models;
namespace MoviesE_commerce.ViewModels
{
	public class MovieViewModel
	{
		[Required]
		public string Title { get; set; }

		public string Description { get; set; }

		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
		public double Price { get; set; }

		public string ImageURL { get; set; }

		[Required]
		//[Display(Name = "Duration (hh:mm:ss)")]
		public TimeSpan Duration { get; set; }

		[Required]
		[Display(Name = "Release Date")]
		public DateTime Date { get; set; }

		[Required]
		public MovieCategory MovieCategory { get; set; }

		[Required]
		[Display(Name = "Producer")]
		public int ProducerId { get; set; }

		public IEnumerable<SelectListItem> Producers { get; set; }

		[Required]
		[Display(Name = "Actors")]
		public List<int> SelectedActorIds { get; set; }

		public IEnumerable<SelectListItem> Actors { get; set; }
	}
}