using Aviate.Core.Enums;

namespace Aviate.Core.Filters
{
    // ================= BOOKING =================
    public class BookingAdminFilter
    {
        public Guid? UserId { get; set; }                // Фільтр за користувачем
        public Guid? FlightId { get; set; }              // Фільтр за рейсом
        public BookingStatus? Status { get; set; }       // Pending, Confirmed, Canceled, тощо
        public decimal? MinTotalPrice { get; set; }
        public decimal? MaxTotalPrice { get; set; }
        public DateTimeOffset? BookingFrom { get; set; }
        public DateTimeOffset? BookingTo { get; set; }
        public string? SortBy { get; set; }              // "BookingDate", "TotalPrice", "Status"
        public bool SortDesc { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}

