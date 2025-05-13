using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.DataAccess.Repositories
{
    public interface IAirportRepository
    {
        Task AddAsync(Airport airport);
        Task DeleteAsync(Airport airport);
        Task<Airport?> GetByIdAsync(Guid id);
        Task<Airport?> GetByCode(string code);
        Task<PagedResult<Airport>> GetFilteredAsync(AirportFilter filter);
        Task UpdateAsync(Airport airport);
    }
}