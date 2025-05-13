using Aviate.Core.Enums;

namespace Aviate.Core.Filters
{
    // ================= SEAT =================
    public class SeatFilter
    {
        public Guid? FlightId { get; set; }              // Пошук по рейсу
        public SeatClass? Class { get; set; }            // Економ / Бізнес
        public bool? IsBooked { get; set; }              // Зайняте місце
        public string? Search { get; set; }              // Пошук по номеру місця
        public string? SortBy { get; set; }              // "SeatNumber", "Class", "IsBooked"
        public bool SortDesc { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}

