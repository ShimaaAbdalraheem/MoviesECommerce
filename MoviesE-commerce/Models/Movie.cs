using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesE_commerce.Models
{
    public enum MovieCategory
    {
        Action,
        Comedy,
        Family,
        Adventure,
        Fantasy,
        Romance,
        Documentary
    }// search
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public string ImageURL { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Duration { get; set; }

        public MovieCategory MovieCategory { get; set; }

        [ForeignKey("ProducerId")]
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
        public virtual List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public virtual ICollection<ActorMovie> ActorMovies { get; set; }
        public virtual ICollection<OrderItem>  OrderItems { get; set; }

    }
}
