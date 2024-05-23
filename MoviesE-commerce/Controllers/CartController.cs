using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesE_commerce.DBContect;
using MoviesE_commerce.Models;

namespace MoviesE_commerce.Controllers
{
    public class CartController : Controller
    {
        int amount=0;
        int Price;
        public long Card_Number { get; set; }
        public DateTime Expiry_ { get; set; }
        public string Cvv_ { get; set; } 


        private readonly MovieE_CommerceDbContext _db;
        public CartController(MovieE_CommerceDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult AddToCart(int movieId, int userId)
        {
            ViewBag.UserId = userId;
            //new
            int id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            ViewBag.Id = id;
            ViewBag.UserName = user.FirstName;
            ViewBag.ImageURL = user.ImageURL;


            Movie movie = _db.Movies.FirstOrDefault(m => m.Id == movieId);

            if (movie == null)
            {

                return NotFound();
            }

            int unitPrice = movie.Price;
            
            int UserId = userId;

            OrderItem orderItem = new OrderItem
            {
                MovieId = movieId,
                Price = unitPrice
            };

            Order order = CreateOrder(userId, orderItem.Price);

            orderItem.OrderId = order.Id; 
            _db.OrderItems.Add(orderItem);
            _db.SaveChanges();



            //List<OrderItem> OrderItems = _db.OrderItems.Where(item => item.OrderId == order.Id).ToList();


            List<OrderItem> OrderItems = _db.OrderItems
               .Include(item => item.Movie) // Include the related Movie entity
               .Where(item => item.OrderId == order.Id)
               .ToList();
            ViewBag.OrderId = order.Id;

            return View("AddToCart", OrderItems);
        }

        private Order CreateOrder(int userId, int price)
        {
            Order existingOrder = _db.Orders.FirstOrDefault(o => o.UserId == userId && o.Status == status.open);

            if (existingOrder != null)
            {
                existingOrder.Amount += 1;
                existingOrder.TotalPrice += price;
                return existingOrder; 
            }
            else
            {
                Price = price;
                amount = 1;
            }

            Order newOrder = new Order
            {
                UserId = userId,
                CreatedDate = DateTime.Now, 
                Status = status.open, 
                Amount = amount,
                TotalPrice = Price,
            };

            _db.Orders.Add(newOrder);
            _db.SaveChanges();

            return newOrder; 
        }
        public ActionResult Orders()
        {

            int id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            ViewBag.Id = id;
            ViewBag.UserName = user.FirstName;
            ViewBag.ImageURL = user.ImageURL;

            var ordersWithItemsAndMovies = _db.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Movie)
            .Where(o => o.UserId == id) 
            .ToList();


            List<Order> orders = _db.Orders.Include(o => o.User).ToList();
            return View(ordersWithItemsAndMovies);
        }
        public IActionResult MyCart()
        {

            int id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            ViewBag.Id = id;
            ViewBag.UserName = user.FirstName;
            ViewBag.ImageURL = user.ImageURL;
            // Retrieve the order with the given user ID and status 'open'
            var order = _db.Orders.FirstOrDefault(o => o.UserId == user.Id && o.Status == status.open);

            if (order == null)
            {
                // Handle case where the order is not found
                return View("NoOrder");
            }

            // Retrieve all order items associated with the found order
            List<OrderItem> OrderItems = _db.OrderItems
               .Include(item => item.Movie) // Include the related Movie entity
               .Where(item => item.OrderId == order.Id)
               .ToList();
            ViewBag.OrderId = order.Id;

            if (OrderItems == null || OrderItems.Count == 0)
            {
                // Handle case where no order items are found
                return NotFound();
            }

            // If everything is found, return the view with the list of order items
            return View(OrderItems);
        }

        public IActionResult CompleteOrder()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            ViewBag.Id = id;
            ViewBag.UserName = user.FirstName;
            ViewBag.ImageURL = user.ImageURL;

            return View();
        }

       

        [HttpPost]

        public IActionResult DeleteItem(int orderitemid)
        {
            try
            {
                int id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

                var user = _db.Users.FirstOrDefault(u => u.Id == id);
                ViewBag.Id = id;
                ViewBag.UserName = user.FirstName;
                ViewBag.ImageURL = user.ImageURL;

                OrderItem orderitem = _db.OrderItems.FirstOrDefault(o => o.Id == orderitemid);
                if (orderitem == null)
                {
                    return NotFound();
                }
                Order existingorder = _db.Orders.SingleOrDefault(order => order.Id == orderitem.OrderId);
                if (existingorder == null)
                {
                    return NotFound();
                }
                if (existingorder.Amount == 1)
                {
                    _db.Orders.Remove(existingorder);
                    _db.SaveChanges();
                }
                else
                {
                    existingorder.Amount -= 1;
                    existingorder.TotalPrice -= orderitem.Price;
                    _db.SaveChanges();
                }
                _db.OrderItems.Remove(orderitem);
                _db.SaveChanges();

                return RedirectToAction("MyCart");

            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle the concurrency exception
                var entry = ex.Entries.Single();
                var clientValues = entry.Entity;

                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    // The entity was deleted by another user
                    // ModelState.AddModelError(string.Empty, "The item was deleted by another user.");
                }
                else
                {
                    // The entity was updated by another user, refresh the entity and notify the user
                    entry.OriginalValues.SetValues(databaseEntry);
                    // ModelState.AddModelError(string.Empty, "The item was modified by another user. Your changes have been discarded.");
                }

                // Optionally, you can retry the operation or notify the user to retry
                return View("NoOrder"); // Adjust the view name and model as necessary
            }

        }

        [HttpGet]
        public IActionResult Payment()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            ViewBag.Id = id;
            ViewBag.UserName = user.FirstName;
            ViewBag.ImageURL = user.ImageURL;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Payment(IFormCollection req, int orderId ,int userID)
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            ViewBag.Id = id;
            ViewBag.UserName = user.FirstName;
            ViewBag.ImageURL = user.ImageURL;

            Card_Number = long.Parse(req["CardNumber"]); 
                Expiry_ =DateTime.Parse (req["Expiry"]);   
                Cvv_ = req["Cvv"];

            var order = _db.Orders.FirstOrDefault(o => o.UserId ==userID && o.Status == status.open);

            if (order == null)
            {
                return NotFound();
            }
            order.Status = status.close;
            _db.SaveChanges();

            Payment payment = new Payment
            {
                PaymentDate = DateTime.Now,
                CardNumber = Card_Number, 
                Expiry =Expiry_,    
                Cvv = Cvv_,
               

            };

            payment.UserId = userID;
            payment.OrderId = orderId;
            _db.Payments.Add(payment);
            _db.SaveChanges();
            ViewData["Message"] = "Payment done successfully";

            Bill bill = new Bill
            {
                PaymentId = payment.Id,
            };
            _db.Bills.Add(bill);
            _db.SaveChanges();
           

            return View("CompleteOrder"); 
        }




    }
}
