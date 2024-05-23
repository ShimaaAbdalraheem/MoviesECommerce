using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesE_commerce.Models
{
    public class Bill
    {
        public int Id { get; set; }//
        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public Payment Payment { get; set; }

    }
}
