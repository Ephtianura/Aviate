using Aviate.Core.Enums;

namespace Aviate.Core.Models
{
    // ================= AIRPLANE =================
    public class Airplane
    {
        private Airplane
            (
            string model, 
            string registrationNumber, 
            int capacity, 
            DateTimeOffset manufactureDate
            )
        {
            Id = Guid.NewGuid() ;
            Model = model.Trim();
            RegistrationNumber = registrationNumber.Trim().ToUpperInvariant();
            Capacity = capacity;
            Status = AirplaneStatus.Active;
            ManufactureDate = manufactureDate;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; private set; }
        public string Model { get; private set; }
        public string RegistrationNumber { get; private set; }
        public int Capacity { get; private set; }
        public AirplaneStatus Status { get; private set; }
        public DateTimeOffset ManufactureDate { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        public static Airplane Create(string model, string registrationNumber, int capacity, DateTimeOffset manufactureDate)
        {
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model cannot be empty");

            if (string.IsNullOrWhiteSpace(registrationNumber))
                throw new ArgumentException("Registration number cannot be empty");

            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than zero");

            if (manufactureDate > DateTimeOffset.UtcNow)
                throw new ArgumentException("Manufacture date cannot be in the future");

            return new Airplane(model, registrationNumber, capacity, manufactureDate);
        }

        public void ChangeModel(string newModel)
        {
            if (string.IsNullOrWhiteSpace(newModel))
                throw new ArgumentException("Model cannot be empty");
            Model = newModel.Trim();
            Touch();
        }
        public void ChangeRegistrationNumber(string newRegistrationNumber)
        {
            if (string.IsNullOrWhiteSpace(newRegistrationNumber))
                throw new ArgumentException("RegistrationNumber cannot be empty");
            RegistrationNumber = newRegistrationNumber.Trim().ToUpperInvariant();
            Touch();
        }
        public void ChangeCapacity(int newCapacity)
        {
            if (newCapacity <= 0)
                throw new ArgumentException("Capacity must be greater than zero");
            Capacity = newCapacity;
            Touch();
        }
        public void ChangeStatus(AirplaneStatus newStatus)
        {
            if (!Enum.IsDefined(typeof(AirplaneStatus), newStatus))
                throw new ArgumentException("Invalid airplane status");
            Status = newStatus;
            Touch();
        }
        public void ChangeManufactureDate(DateTimeOffset newManufactureDate)
        {
            if (newManufactureDate > DateTimeOffset.UtcNow)
                throw new ArgumentException("Manufacture date cannot be in the future");
            ManufactureDate = newManufactureDate;
            Touch();
        }

        private void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
        public override string ToString() => $"{Model} ({RegistrationNumber}) — {Status}, Capacity: {Capacity}";
    }
}
