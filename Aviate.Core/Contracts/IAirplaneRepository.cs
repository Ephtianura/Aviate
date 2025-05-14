using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.DataAccess.Repositories
{
    public interface IAirplaneRepository
    {
        
        Task<Airplane?> GetByIdAsync(Guid id);
        Task<Airplane?> GetByRegistrationAsync(string registrationNumber);
        Task<PagedResult<Airplane>> GetFilteredAsync(AirplaneFilter filter);
        Task AddAsync(Airplane airplane);
        Task UpdateAsync(Airplane airplane);
        Task DeleteAsync(Airplane airplane);

    }
}