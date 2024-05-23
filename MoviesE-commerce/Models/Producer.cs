using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MoviesE_commerce.Models
{
    public class Producer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureURL { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }


    }
}
