namespace MoviesE_commerce.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public userRole Role { get; set; }
    }
}
