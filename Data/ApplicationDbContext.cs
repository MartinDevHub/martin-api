using Marcoff_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>().HasData(
                new Booking()
                {
                    Id =3,
                    Name = "Ricardo",
                    Detail = "Abonó 10%",
                    Fee = 200.0,
                    BedsOcuppied = 3,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,

                },
                  new Booking()
                  {
                      Id = 4,
                      Name = "Romualdo",
                      Detail = "Abonó 10%",
                      Fee = 150.0,
                      BedsOcuppied = 4,
                      DateCreated = DateTime.Now,
                      DateModified = DateTime.Now,

                  }
                );
        }
    }
}
