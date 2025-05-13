namespace Aviate.Core.Filters
{
    public class AirportFilter
    {
        public string? Search { get; set; }           // Пошук за Name або Code аеропорту
        public string? Country { get; set; }          // Фільтр за країною
        public string? City { get; set; }             // Фільтр за містом
        public string? SortBy { get; set; }           // Поле для сортування — "Name", "Code", "City", "Country"
        public bool SortDesc { get; set; }            // Зворотне сортування
        public int Page { get; set; } = 1;            // Номер сторінки
        public int PageSize { get; set; } = 20;       // Кількість елементів на сторінці
    }
}
