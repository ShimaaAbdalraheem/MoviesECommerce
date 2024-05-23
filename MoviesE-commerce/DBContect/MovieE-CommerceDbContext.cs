using Microsoft.EntityFrameworkCore;
using MoviesE_commerce.Models;

namespace MoviesE_commerce.DBContect
{
    public class MovieE_CommerceDbContext:DbContext
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
         
         public DbSet<ActorMovie> ActorMovies { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public  DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<User> Users { get; set; }

        public MovieE_CommerceDbContext(DbContextOptions<MovieE_CommerceDbContext> options)
        : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MoviesE_commerce;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        //    base.OnConfiguring(optionsBuilder); 
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<OrderItem>()
            //     .HasKey(y => new { y.OrderId, y.MovieId});
            //modelBuilder.Entity<OrderItem>()
            //            .HasOne(crs => crs.Movie)
            //            .WithMany(c => c.OrderItems)
            //            .HasForeignKey(crs => crs.MovieId);
            //modelBuilder.Entity<OrderItem>()
            //             .HasOne(crs => crs.Order)
            //             .WithMany(c => c.OrderItems)
            //             .HasForeignKey(crs => crs.OrderId);


            modelBuilder.Entity<OrderItem>()
           .HasKey(y => y.Id); 

            modelBuilder.Entity<OrderItem>()
                .HasOne(crs => crs.Movie)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(crs => crs.MovieId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(crs => crs.Order)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(crs => crs.OrderId);





            modelBuilder.Entity<ActorMovie>()
                .HasKey(y => new { y.ActorId, y.MovieId });
            modelBuilder.Entity<ActorMovie>()
                        .HasOne(crs => crs.Movie)
                        .WithMany(c => c.ActorMovies)
                        .HasForeignKey(crs => crs.MovieId);
            modelBuilder.Entity<ActorMovie>()
                         .HasOne(crs => crs.Actor)
                         .WithMany(c => c.ActorMovies)
                         .HasForeignKey(crs => crs.ActorId);


            //     modelBuilder.Entity<Bill>()
            //.HasOne(b => b.Payment) // Bill has one Payment
            //.WithOne(p => p.Bill)   // Payment has one associated Bill
            //.HasForeignKey<Bill>(p => p.PaymentId);

            modelBuilder.Entity<User>()
            .HasIndex(c => new { c.Email })
            .IsUnique(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
