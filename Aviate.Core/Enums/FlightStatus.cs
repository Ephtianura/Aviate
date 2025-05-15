namespace Aviate.Core.Enums
{
    // Статус рейсу
    public enum FlightStatus
    {
        Scheduled = 0, // Заплановано
        InFlight = 1, // У польоті
        Delayed = 2, // Перенесено
        Cancelled = 3, // Скасовано
        Completed = 4 // Завершено
    }
}
