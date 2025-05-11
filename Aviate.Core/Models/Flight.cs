using Aviate.Core.Enums;

namespace Aviate.Core.Models
{

    public class Flight
    {
        private Flight(Guid airplaneId, Guid departureAirportId, Guid arrivalAirportId, 
            string flightNumber, decimal basePrice, 
            DateTimeOffset departureTime, DateTimeOffset arrivalTime)
        {
            Id = Guid.NewGuid();
            AirplaneId = airplaneId;
            DepartureAirportId = departureAirportId;
            ArrivalAirportId = arrivalAirportId;
            FlightNumber = flightNumber.Trim();
            BasePrice = basePrice;
            Status = FlightStatus.Scheduled;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; private set; }

        // Ключи літака та аеропортів
        public Guid AirplaneId { get; private set; }
        public Guid DepartureAirportId { get; private set; }
        public Guid ArrivalAirportId { get; private set; }

        // Номер рейсу та ціна
        public string FlightNumber { get; private set; }
        public decimal BasePrice { get; private set; }

        // Статус
        public FlightStatus Status { get; private set; }

        // Дата відправки та прибуття
        public DateTimeOffset DepartureTime { get; private set; }
        public DateTimeOffset ArrivalTime { get; private set; }

        // Дата Створення та оновлення
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        // Навігація
        public Airplane Airplane { get; private set; } = null!;
        public Airport DepartureAirport { get; private set; } = null!;
        public Airport ArrivalAirport { get; private set; } = null!;

        private List<Seat> _seats = new();
        public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();


        public static Flight Create(
            Guid airplaneId, Guid departureAirportId, Guid arrivalAirportId,
            string flightNumber, decimal basePrice,
            DateTimeOffset departureTime, DateTimeOffset arrivalTime)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
                throw new ArgumentException("FlightNumber cannot be empty");
            if (basePrice < 0)
                throw new ArgumentException("The price cannot be negative");
            if (departureTime > arrivalTime)
                throw new ArgumentException("Departure time cannot be later than arrival time");
            if (departureAirportId == arrivalAirportId)
                throw new ArgumentException("Departure and arrival airports cannot be the same");

            return new Flight(airplaneId, departureAirportId, arrivalAirportId,flightNumber, basePrice, departureTime, arrivalTime);
        }

        // ===================== Оновлення =====================
        public void ChangeFlightNumber(string newFlightNumber)
        {
            if (string.IsNullOrWhiteSpace(newFlightNumber))
                throw new ArgumentException("FlightNumber cannot be empty");
            FlightNumber = newFlightNumber.Trim();
            Touch();
        }

        public void ChangeBasePrice(decimal newBasePrice)
        {
            if (newBasePrice < 0)
                throw new ArgumentException("The price cannot be negative");
            BasePrice = newBasePrice;
            Touch();
        }
        public void ChangeStatus(FlightStatus newStatus)
        {
            if (!Enum.IsDefined(typeof(FlightStatus), newStatus))
                throw new ArgumentException("Invalid flight status");
            Status = newStatus;
            Touch();
        }

        public void ChangeSchedule(DateTimeOffset newDepartureTime, DateTimeOffset newArrivalTime)
        {
            if (newDepartureTime >= newArrivalTime)
                throw new ArgumentException("Departure time must be earlier than arrival time.");
            DepartureTime = newDepartureTime;
            ArrivalTime = newArrivalTime;
            Touch();
        }

        public void AddSeat(string seatNumber, SeatClass seatClass)
        {
            if (_seats.Any(s => s.SeatNumber == seatNumber))
                throw new ArgumentException("Seat already exists");
            _seats.Add(new Seat(this.Id, seatNumber, seatClass));
        }

        // ===================== Навігація =====================
        // Назначити літак
        public void AssignAirplane(Airplane airplane)
        {
            Airplane = airplane ?? throw new ArgumentNullException(nameof(airplane));
            AirplaneId = airplane.Id;
            Touch();
        }

        // Назначити пункт відправки
        public void AssignDepartureAirport(Airport airport)
        {
            DepartureAirport = airport ?? throw new ArgumentNullException(nameof(airport));
            DepartureAirportId = airport.Id;
            Touch();
        }

        // Назначити пункт прибуття
        public void AssignArrivalAirport(Airport airport)
        {
            ArrivalAirport = airport ?? throw new ArgumentNullException(nameof(airport));
            ArrivalAirportId = airport.Id;
            Touch();
        }

        // ===================== Генерація місць =====================
        private void GenerateSeats(int economyCount, int businessCount, int firstClassCount)
        {
            int totalSeats = economyCount + businessCount + firstClassCount;
            if (Airplane == null)
                throw new InvalidOperationException("Airplane must be assigned before generating seats.");
            if (totalSeats > Airplane.Capacity)
                throw new ArgumentException("Total seats exceed airplane capacity.");

            _seats.Clear();
            int seatIndex = 1;

            for (int i = 0; i < economyCount; i++)
                _seats.Add(new Seat(this.Id, $"E{seatIndex++}", SeatClass.Economy));

            for (int i = 0; i < businessCount; i++)
                _seats.Add(new Seat(this.Id, $"B{seatIndex++}", SeatClass.Business));

            for (int i = 0; i < firstClassCount; i++)
                _seats.Add(new Seat(this.Id, $"F{seatIndex++}", SeatClass.First));

            Touch();
        }

        private void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
        public override string ToString() => $"{FlightNumber} ({DepartureAirport?.Code ?? DepartureAirportId} - {ArrivalAirport?.Code ?? ArrivalAirportId}) — {Status}, BasePrice: {BasePrice}";
    }
}
