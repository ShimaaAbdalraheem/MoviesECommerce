using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesE_commerce.Models
{
    public class ActorMovie
    {
        [ForeignKey("ActorId")]
        public int ActorId { get; set; }//
        public Actor Actor { get; set; }
        [ForeignKey("MovieId")]
        public int MovieId { get; set; }//
        public Movie Movie { get; set; }
        
    }
}
