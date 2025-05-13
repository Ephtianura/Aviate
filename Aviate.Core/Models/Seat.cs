using Aviate.Core.Enums;

namespace Aviate.Core.Models
{
    // ================= SEAT =================
    public class Seat
    {
        private Seat() { }
        private Seat(Guid flightId, string seatNumber, SeatClass seatClass)
        {
            Id = Guid.NewGuid();
            FlightId = flightId;
            SeatNumber = seatNumber.Trim().ToUpperInvariant();
            Class = seatClass;
            IsBooked = false;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; private set; }
        public Guid FlightId { get; private set; }

        public string SeatNumber { get; private set; } = null!;
        public SeatClass Class { get; private set; }
        public bool IsBooked { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        // Навігація
        public Flight Flight { get; private set; } = null!;

        // ===================== Фабричний метод створення місця =====================
        public static Seat Create(Guid flightId, string seatNumber, SeatClass seatClass)
        {
            if (flightId == Guid.Empty)
                throw new ArgumentException("FlightId cannot be empty.");
            if (string.IsNullOrWhiteSpace(seatNumber))
                throw new ArgumentException("Seat number cannot be empty.");

            return new Seat(flightId, seatNumber, seatClass);
        }

        // ===================== Методи поведінки =====================

        public void Book()
        {
            if (IsBooked)
                throw new InvalidOperationException($"Seat {SeatNumber} is already booked.");
            IsBooked = true;
            Touch();
        }

        public void Unbook()
        {
            if (!IsBooked)
                throw new InvalidOperationException($"Seat {SeatNumber} is not booked.");
            IsBooked = false;
            Touch();
        }

        public void ChangeSeatClass(SeatClass newClass)
        {
            if (!Enum.IsDefined(typeof(SeatClass), newClass))
                throw new ArgumentException("Invalid seat class.");
            Class = newClass;
            Touch();
        }

        public void Rename(string newSeatNumber)
        {
            if (string.IsNullOrWhiteSpace(newSeatNumber))
                throw new ArgumentException("Seat number cannot be empty.");
            SeatNumber = newSeatNumber.Trim().ToUpperInvariant();
            Touch();
        }

        private void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
        public override string ToString() =>
            $"{SeatNumber} ({Class}) — {(IsBooked ? "Booked" : "Available")}";
    }
}
