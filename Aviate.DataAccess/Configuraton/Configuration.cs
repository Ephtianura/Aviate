using Aviate.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviate.DataAccess.Configurations
{
    // ===================== USER =====================
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(u => u.PasswordHash)
                .IsRequired();
            builder.Property(u => u.Phone)
                .HasMaxLength(20)
                .IsRequired(false);
            builder.Property(u => u.Role)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(u => u.RegistrationDate)
                .IsRequired();
            builder.Property(u => u.UpdatedDate)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasMany<Booking>()
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    // ===================== AIRPORT =====================
    public class AirportConfiguration : IEntityTypeConfiguration<Airport>
    {
        public void Configure(EntityTypeBuilder<Airport> builder)
        {
            builder.ToTable("Airports");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(a => a.Code)
                .IsRequired()
                .HasMaxLength(10);
            builder.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a => a.CreatedAt).IsRequired();
            builder.Property(a => a.UpdatedAt).IsRequired();

            builder.HasIndex(a => a.Code).IsUnique();
        }
    }

    // ===================== AIRPLANE =====================
    public class AirplaneConfiguration : IEntityTypeConfiguration<Airplane>
    {
        public void Configure(EntityTypeBuilder<Airplane> builder)
        {
            builder.ToTable("Airplanes");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Model)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a => a.RegistrationNumber)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(a => a.Capacity)
                .IsRequired();
            builder.Property(a => a.Status)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(a => a.ManufactureDate)
                .IsRequired();
            builder.Property(a => a.CreatedAt).IsRequired();
            builder.Property(a => a.UpdatedAt).IsRequired();

            builder.HasIndex(a => a.RegistrationNumber).IsUnique();

            builder.HasMany<Flight>()
                .WithOne(f => f.Airplane)
                .HasForeignKey(f => f.AirplaneId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    // ===================== FLIGHT =====================
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.ToTable("Flights");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.FlightNumber)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(f => f.BasePrice)
                .HasPrecision(10, 2)
                .IsRequired();
            builder.Property(f => f.Status)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(f => f.DepartureTime).IsRequired();
            builder.Property(f => f.ArrivalTime).IsRequired();
            builder.Property(f => f.CreatedAt).IsRequired();
            builder.Property(f => f.UpdatedAt).IsRequired();

            builder.HasOne(f => f.Airplane)
                .WithMany()
                .HasForeignKey(f => f.AirplaneId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.DepartureAirport)
                .WithMany()
                .HasForeignKey(f => f.DepartureAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.ArrivalAirport)
                .WithMany()
                .HasForeignKey(f => f.ArrivalAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(f => f.FlightNumber)
                .IsUnique();
        }
    }

    // ===================== SEAT =====================
    public class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.ToTable("Seats");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.SeatNumber)
                .IsRequired()
                .HasMaxLength(10);
            builder.Property(s => s.Class)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(s => s.IsBooked)
                .IsRequired();
            builder.Property(s => s.CreatedAt).IsRequired();
            builder.Property(s => s.UpdatedAt).IsRequired();

            builder.HasOne(s => s.Flight)
                .WithMany(f => f.Seats)
                .HasForeignKey(s => s.FlightId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => new { s.FlightId, s.SeatNumber })
                .IsUnique();
        }
    }

    // ===================== BOOKING =====================
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.TotalPrice)
                .HasPrecision(10, 2)
                .IsRequired();
            builder.Property(b => b.Status)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(b => b.BookingDate).IsRequired();
            builder.Property(b => b.UpdatedAt).IsRequired();

            builder.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Flight)
                .WithMany()
                .HasForeignKey(b => b.FlightId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Seat)
                .WithMany()
                .HasForeignKey(b => b.SeatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    // ===================== PAYMENT =====================
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Method)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(p => p.Status)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(p => p.Amount)
                .HasPrecision(10, 2)
                .IsRequired();
            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.UpdatedAt).IsRequired();

            builder.HasOne(p => p.Booking)
                .WithMany()
                .HasForeignKey(p => p.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
