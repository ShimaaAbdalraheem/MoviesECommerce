using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesE_commerce.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }//
        public int Price { get; set;}
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; } 

    }
}
