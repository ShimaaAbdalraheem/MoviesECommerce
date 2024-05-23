using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesE_commerce.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
       
        //public int OrderItemId { get; set; }
        //[ForeignKey("OrderItemId")]
        public int OrderId { get; set; }
        public Order Order { get; set; }    
        public long CardNumber {  get; set; }
        public DateTime Expiry { get; set; }
        public string Cvv { get; set; }
        public int UserId { get; set; }
        public User  User { get; set; }
        public Bill Bill { get; set; }

    }
}
