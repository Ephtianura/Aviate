using Aviate.Core.Filters;
using Aviate.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Aviate.DataAccess.Repositories
{
    // ===================== FLIGHT =====================
    public class FlightRepository : IFlightRepository
    {
        private readonly AviateDbContext _dbContext;
        public FlightRepository(AviateDbContext db) => _dbContext = db;

        public async Task<bool> ExistsAsync(string flightNumber)
        {
            return await _dbContext.Flights.AnyAsync(f => f.FlightNumber == flightNumber);
        }
        public async Task<bool> ExistsForAirplaneAtTimeAsync(Guid airplaneId, DateTimeOffset departureTime)
        {
            return await _dbContext.Flights.AnyAsync(f => f.AirplaneId == airplaneId && f.DepartureTime == departureTime);
        }

        public async Task<bool> ExistsAsync(Guid airplaneId, Guid departureAirportId, Guid arrivalAirportId, DateTimeOffset departureTime)
        {
            return await _dbContext.Flights.AnyAsync(f =>
                f.AirplaneId == airplaneId &&
                f.DepartureAirportId == departureAirportId &&
                f.ArrivalAirportId == arrivalAirportId &&
                f.DepartureTime == departureTime
            );
        }


        public async Task<Flight?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Flights
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Flight?> GetByFlightNumberAsync(string flightNumber)
        {
            return await _dbContext.Flights
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .FirstOrDefaultAsync(f => f.FlightNumber == flightNumber.Trim().ToUpperInvariant());
        }

        public async Task<PagedResult<Flight>> GetFilteredAsync(FlightFilter filter)
        {
            // Инициализируем запрос через Raw SQL с оконной функцией.
            // Выбираем только те рейсы, которые входят в ТОП-5 для каждого дня.
            var query = _dbContext.Flights
                .FromSqlRaw(@"
            WITH RankedFlights AS (
                SELECT *, 
                       ROW_NUMBER() OVER (
                           PARTITION BY CAST(""DepartureTime"" AS DATE) 
                           ORDER BY ""DepartureTime"" ASC
                       ) as RowNum
                FROM ""Flights""
            )
            SELECT * FROM RankedFlights 
            WHERE RowNum <= 5")
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .AsQueryable();

            // 1. Текстовый поиск
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim().ToLower();
                query = query.Where(
                    f => f.FlightNumber.ToLower().Contains(term) ||
                    f.ArrivalAirport.City.ToLower().Contains(term) ||
                    f.ArrivalAirport.Country.ToLower().Contains(term) ||
                    f.DepartureAirport.City.ToLower().Contains(term) ||
                    f.DepartureAirport.Country.ToLower().Contains(term));
            }

            // 2. Фильтры по Сущностям
            if (filter.AirplaneId.HasValue)
                query = query.Where(f => f.AirplaneId == filter.AirplaneId);

            if (filter.DepartureAirportId.HasValue)
                query = query.Where(f => f.DepartureAirportId == filter.DepartureAirportId);

            if (filter.ArrivalAirportId.HasValue)
                query = query.Where(f => f.ArrivalAirportId == filter.ArrivalAirportId);

            if (filter.Status.HasValue)
                query = query.Where(f => f.Status == filter.Status);

            // 3. Фильтры даты отправления (Departure) + ExcludeExpired
            if (filter.ExcludeExpired)
            {
                var effectiveDepartureFrom = filter.DepartureFrom.HasValue && filter.DepartureFrom.Value > DateTimeOffset.UtcNow
                    ? filter.DepartureFrom.Value
                    : DateTimeOffset.UtcNow;

                query = query.Where(f => f.DepartureTime > effectiveDepartureFrom);
            }
            else if (filter.DepartureFrom.HasValue)
            {
                query = query.Where(f => f.DepartureTime >= filter.DepartureFrom.Value);
            }

            if (filter.DepartureTo.HasValue)
                query = query.Where(f => f.DepartureTime <= filter.DepartureTo.Value);

            // 4. Фильтры даты прибытия (Arrival) + ExcludeExpired
            if (filter.ExcludeExpired)
            {
                var effectiveArrivalFrom = filter.ArrivalFrom.HasValue && filter.ArrivalFrom.Value > DateTimeOffset.UtcNow
                    ? filter.ArrivalFrom.Value
                    : DateTimeOffset.UtcNow;

                query = query.Where(f => f.ArrivalTime > effectiveArrivalFrom);
            }
            else if (filter.ArrivalFrom.HasValue)
            {
                query = query.Where(f => f.ArrivalTime >= filter.ArrivalFrom.Value);
            }

            if (filter.ArrivalTo.HasValue)
                query = query.Where(f => f.ArrivalTime <= filter.ArrivalTo.Value);

            // 5. Динамическая сортировка списка
            query = filter.SortBy?.ToLower() switch
            {
                "flightnumber" => filter.SortDesc ? query.OrderByDescending(f => f.FlightNumber) : query.OrderBy(f => f.FlightNumber),
                "departuretime" => filter.SortDesc ? query.OrderByDescending(f => f.DepartureTime) : query.OrderBy(f => f.DepartureTime),
                "arrivaltime" => filter.SortDesc ? query.OrderByDescending(f => f.ArrivalTime) : query.OrderBy(f => f.ArrivalTime),
                "baseprice" => filter.SortDesc ? query.OrderByDescending(f => f.BasePrice) : query.OrderBy(f => f.BasePrice),
                _ => query.OrderBy(f => f.FlightNumber)
            };

            // 6. Подсчет общего количества (база сделает COUNT поверх CTE-запроса)
            var totalCount = await query.CountAsync();

            // 7. Постраничный вывод (база добавит OFFSET и FETCH/LIMIT автоматически)
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<Flight>(items, totalCount, filter.Page, filter.PageSize);
        }


        public async Task AddAsync(Flight flight)
        {
            await _dbContext.Flights.AddAsync(flight);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeFastAsync(List<Flight> flights)
        {
            await _dbContext.Flights.AddRangeAsync(flights);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Flight flight)
        {
            _dbContext.Flights.Update(flight);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Flight flight)
        {
            _dbContext.Flights.Remove(flight);
            await _dbContext.SaveChangesAsync();
        }
    }
}

