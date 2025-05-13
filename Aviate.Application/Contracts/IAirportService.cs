using Aviate.Application.Dto.Airport;
using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.Application.Contracts
{
    public interface IAirportService
    {
        Task<Airport> CreateAsync(AirportCreateDto request);
        Task DeleteAsync(Guid id);
        Task<Airport> GetByIdAsync(Guid id);
        Task<PagedResult<Airport>> GetFilteredAsync(AirportFilter filter);
        Task UpdateAsync(Guid id, AirportUpdateDto request);
    }
}