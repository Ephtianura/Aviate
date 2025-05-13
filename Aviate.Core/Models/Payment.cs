using Aviate.Core.Enums;

namespace Aviate.Core.Models
{
    // ===================== PAYMENT =====================
    public class Payment
    {        
        private Payment() { }
        private Payment(Booking booking, PaymentMethod method, decimal amount)
        {
            Id = Guid.NewGuid();
            AssignBooking(booking);
            Method = method;
            Amount = amount;
            Status = PaymentStatus.Pending;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; private set; }
        public Guid BookingId { get; private set; }

        public PaymentMethod Method { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentStatus Status { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        // Навигация
        public Booking Booking { get; private set; } = null!;

        // ===================== Фабричний метод створення оплати =====================
        public static Payment Create(Booking booking, PaymentMethod method, decimal amount)
        {
            if (booking == null) throw new ArgumentNullException(nameof(booking));
            if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.");
            return new Payment(booking, method, amount);
        }

        // ===================== Методи =====================
        public void AssignBooking(Booking booking)
        {
            Booking = booking ?? throw new ArgumentNullException(nameof(booking));
            BookingId = booking.Id;
            Touch();
        }

        public void MarkSuccess()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Payment already processed.");
            Status = PaymentStatus.Success;
            Booking.MarkAsPaid();
            Touch();
        }

        public void MarkFailed()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Payment already processed.");
            Status = PaymentStatus.Failed;
            Touch();
        }

        private void Touch() => UpdatedAt = DateTimeOffset.UtcNow;

        public override string ToString() =>
            $"Payment {Id} — {Method}, {Status}, {Amount:C}";
    }
}
