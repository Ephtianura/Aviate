using Aviate.Core.Enums;

namespace Aviate.Core.Filters
{
    // ================= AIRPLANE =================
    public class AirplaneFilter
    {
        public string? Search { get; set; }             // Пошук за моделлю або реєстраційним номером
        public AirplaneStatus? Status { get; set; }     // Фільтр за статусом
        public int? MinCapacity { get; set; }           // Мінімальна кількість місць
        public int? MaxCapacity { get; set; }           // Максимальна кількість місць
        public DateTimeOffset? ManufactureFrom { get; set; } // Вироблено від
        public DateTimeOffset? ManufactureTo { get; set; }   // Вироблено до
        public string? SortBy { get; set; }             // "Model", "RegistrationNumber", "Capacity", "ManufactureDate"
        public bool SortDesc { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}

