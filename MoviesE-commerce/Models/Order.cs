using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesE_commerce.Models
{
   
    public  enum status
    {
        open,
        close
    }
    public class Order
    {
        public int Id { get; set; }//add
        public int Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public long TotalPrice { get; set; }

        public status  Status { get; set; }
        public int UserId { get; set; }//
        public User User { get; set; }
        
        public ShoppingCartItem shoppingCartItem { get; set; }
        public virtual ICollection<OrderItem> OrderItems{ get; set; }
        public Payment Payment { get; set; }
    }
}
