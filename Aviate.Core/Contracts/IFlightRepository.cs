using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.DataAccess.Repositories
{
    public interface IFlightRepository
    {
        
        
        Task<bool> ExistsAsync(string flightNumber);
        Task<bool> ExistsForAirplaneAtTimeAsync(Guid airplaneId, DateTimeOffset departureTime);
        Task<bool> ExistsAsync(Guid airplaneId, Guid departureAirportId, Guid arrivalAirportId, DateTimeOffset departureTime);
        Task<Flight?> GetByFlightNumberAsync(string flightNumber);
        Task<Flight?> GetByIdAsync(Guid id);
        Task<PagedResult<Flight>> GetFilteredAsync(FlightFilter filter);
        Task UpdateAsync(Flight flight);
        Task AddAsync(Flight flight);
        Task DeleteAsync(Flight flight);
        Task AddRangeFastAsync(List<Flight> flights);
    }
}