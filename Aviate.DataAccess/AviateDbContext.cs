using Aviate.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace Aviate.DataAccess
{
    public class AviateDbContext : DbContext
    {
        public AviateDbContext(DbContextOptions<AviateDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Airport> Airports => Set<Airport>();
        public DbSet<Airplane> Airplanes => Set<Airplane>();
        public DbSet<Flight> Flights => Set<Flight>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AviateDbContext).Assembly);
        }
    }

}