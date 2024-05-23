using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesE_commerce.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; } //add

      
        //foreign key from Movie Table
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        //foreign key from User Table
       
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}
