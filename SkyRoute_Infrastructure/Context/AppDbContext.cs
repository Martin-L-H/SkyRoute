using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SkyRoute_Domain.Entities;

namespace SkyRoute_Infrastructure.Context
{
    public class AppDbContext : DbContext
    {

        public DbSet<Airport> Airports { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Passenger> Passengers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
