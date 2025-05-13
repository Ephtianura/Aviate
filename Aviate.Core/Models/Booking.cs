using Aviate.Core.Enums;

namespace Aviate.Core.Models
{
    // ================= BOOKING =================
    public class Booking
    {
        private Booking() { }
        private Booking(User user, Flight flight, Seat seat, decimal totalPrice)
        {
            Id = Guid.NewGuid();
            AssignUser(user);
            AssignFlight(flight);
            AssignSeat(seat);

            TotalPrice = totalPrice;
            Status = BookingStatus.Pending;
            BookingDate = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid FlightId { get; private set; }
        public Guid SeatId { get; private set; }

        public decimal TotalPrice { get; private set; }
        public BookingStatus Status { get; private set; }
        public DateTimeOffset BookingDate { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        // Навігація
        public User User { get; private set; } = null!;
        public Flight Flight { get; private set; } = null!;
        public Seat Seat { get; private set; } = null!;

        // ===================== Фабричний метод створення бронювання =====================
        public static Booking Create(User user, Flight flight, Seat seat, decimal totalPrice)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (flight == null) throw new ArgumentNullException(nameof(flight));
            if (seat == null) throw new ArgumentNullException(nameof(seat));
            if (totalPrice < 0) throw new ArgumentException("Total price cannot be negative.");

            return new Booking(user, flight, seat, totalPrice);
        }


        // ===================== Методи поведінки =====================
        public void AssignUser(User user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            UserId = user.Id;
            Touch();
        }

        public void AssignFlight(Flight flight)
        {
            Flight = flight ?? throw new ArgumentNullException(nameof(flight));
            FlightId = flight.Id;
            Touch();
        }

        public void AssignSeat(Seat seat)
        {
            Seat = seat ?? throw new ArgumentNullException(nameof(seat));
            SeatId = seat.Id;
            seat.Book(); 
            Touch();
        }

        public void MarkAsPaid()
        {
            if (Status == BookingStatus.Cancelled)
                throw new InvalidOperationException("Cannot pay for a cancelled booking.");
            Status = BookingStatus.Paid;
            Touch();
        }

        public void Cancel()
        {
            if (Status == BookingStatus.Cancelled)
                throw new InvalidOperationException("Booking already cancelled.");

            Status = BookingStatus.Cancelled;
            Seat.Unbook(); // Звільнити місце
            Touch();
        }

        private void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
        public override string ToString() =>
            $"Booking {Id} for {User.FullName} — {Status}, {TotalPrice:C}";
    }
}
