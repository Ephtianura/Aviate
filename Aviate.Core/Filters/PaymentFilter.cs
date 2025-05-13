using Aviate.Core.Enums;

namespace Aviate.Core.Filters
{
    // ================= PAYMENT =================
    public class PaymentFilter
    {
        public Guid? BookingId { get; set; }
        public PaymentStatus? Status { get; set; }
        public PaymentMethod? Method { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public DateTimeOffset? CreatedFrom { get; set; }
        public DateTimeOffset? CreatedTo { get; set; }
        public string? SortBy { get; set; }              // "CreatedAt", "Amount", "Status", "Method"
        public bool SortDesc { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}

