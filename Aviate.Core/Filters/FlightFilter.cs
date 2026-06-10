using Aviate.Core.Enums;

namespace Aviate.Core.Filters
{
    // ================= FLIGHT =================
    public class FlightFilter
    {
        public string? Search { get; set; }              // Номер рейсу
        public Guid? AirplaneId { get; set; }            // Фільтр за літаком
        public Guid? DepartureAirportId { get; set; }
        public Guid? ArrivalAirportId { get; set; }
        public FlightStatus? Status { get; set; }
        public DateTimeOffset? DepartureFrom { get; set; }
        public DateTimeOffset? DepartureTo { get; set; }
        public DateTimeOffset? ArrivalFrom { get; set; }
        public DateTimeOffset? ArrivalTo { get; set; }
        public bool ExcludeExpired { get; set; }
        public string? SortBy { get; set; }              // "FlightNumber", "DepartureTime", "ArrivalTime", "BasePrice"
        public bool SortDesc { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20; 

    }
}

