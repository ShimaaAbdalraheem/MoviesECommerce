using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesE_commerce.Models
{
    public enum userRole
    {
        Admin, User
    }
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageURL { get; set; }

        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public userRole userRole { get; set; }
        //navigation 
       
        public ShoppingCartItem ShoppingCartItem { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection <Payment> Payment { get; set; }
    }
}
