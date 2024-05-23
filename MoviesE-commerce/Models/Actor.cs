using System.ComponentModel.DataAnnotations;

namespace MoviesE_commerce.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Name should be between 3 and 50")]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "The Name should be between 3 and 50")]
        public string Bio{ get; set; }
        public string ProfilePictureURL { get; set; }
        public virtual ICollection<ActorMovie> ActorMovies{ get; set; }

    }
}
