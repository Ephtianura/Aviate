using Aviate.Core.Contracts;
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

        public async Task<Flight?> GetByIdAsync(Guid id) =>
            await _dbContext.Flights
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .FirstOrDefaultAsync(f => f.Id == id);


        public async Task<Flight> GetByIdOrThrowAsync(Guid id)
        {
            var flight = await GetByIdAsync(id);
            if (flight == null)
                throw new KeyNotFoundException($"Flight with id {id} not found.");
            return flight;
        }

        public async Task<Flight?> GetByFlightNumberAsync(string flightNumber) =>
            await _dbContext.Flights
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .FirstOrDefaultAsync(f => f.FlightNumber == flightNumber.Trim().ToUpperInvariant());

        public async Task<Flight> GetByFlightNumberOrThrowAsync(string flightNumber)
        {
            var flight = await GetByFlightNumberAsync(flightNumber);
            if (flight == null)
                throw new KeyNotFoundException($"Flight with number {flightNumber} not found.");
            return flight;
        }

        public async Task<PagedResult<Flight>> GetFilteredAsync(FlightFilter filter)
        {
            var query = _dbContext.Flights
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim().ToLower();
                query = query.Where(f => f.FlightNumber.ToLower().Contains(term));
            }

            if (filter.AirplaneId.HasValue)
                query = query.Where(f => f.AirplaneId == filter.AirplaneId);

            if (filter.DepartureAirportId.HasValue)
                query = query.Where(f => f.DepartureAirportId == filter.DepartureAirportId);

            if (filter.ArrivalAirportId.HasValue)
                query = query.Where(f => f.ArrivalAirportId == filter.ArrivalAirportId);

            if (filter.Status.HasValue)
                query = query.Where(f => f.Status == filter.Status);

            if (filter.DepartureFrom.HasValue)
                query = query.Where(f => f.DepartureTime >= filter.DepartureFrom);

            if (filter.DepartureTo.HasValue)
                query = query.Where(f => f.DepartureTime <= filter.DepartureTo);

            if (filter.ArrivalFrom.HasValue)
                query = query.Where(f => f.ArrivalTime >= filter.ArrivalFrom);

            if (filter.ArrivalTo.HasValue)
                query = query.Where(f => f.ArrivalTime <= filter.ArrivalTo);

            query = filter.SortBy?.ToLower() switch
            {
                "flightnumber" => filter.SortDesc ? query.OrderByDescending(f => f.FlightNumber) : query.OrderBy(f => f.FlightNumber),
                "departuretime" => filter.SortDesc ? query.OrderByDescending(f => f.DepartureTime) : query.OrderBy(f => f.DepartureTime),
                "arrivaltime" => filter.SortDesc ? query.OrderByDescending(f => f.ArrivalTime) : query.OrderBy(f => f.ArrivalTime),
                "baseprice" => filter.SortDesc ? query.OrderByDescending(f => f.BasePrice) : query.OrderBy(f => f.BasePrice),
                _ => query.OrderBy(f => f.FlightNumber)
            };

            var totalCount = await query.CountAsync();
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

