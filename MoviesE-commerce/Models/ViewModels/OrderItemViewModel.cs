namespace MoviesE_commerce.Models.ViewModels
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int OrderId { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }


    }
}