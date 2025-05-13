using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Aviate.DataAccess.Repositories
{
    // ===================== AIRPORT =====================
    public class AirportRepository : IAirportRepository
    {
        private readonly AviateDbContext _dbContext;
        public AirportRepository(AviateDbContext db) => _dbContext = db;

        public async Task<Airport?> GetByIdAsync(Guid id) =>
            await _dbContext.Airports.FindAsync(id);

        public async Task<PagedResult<Airport>> GetFilteredAsync(AirportFilter filter)
        {
            var query = _dbContext.Airports.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim().ToLower();
                query = query.Where(a =>
                    a.Name.ToLower().Contains(term) ||
                    a.Code.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(filter.Country))
                query = query.Where(a => a.Country.ToLower() == filter.Country.Trim().ToLower());

            if (!string.IsNullOrWhiteSpace(filter.City))
                query = query.Where(a => a.City.ToLower() == filter.City.Trim().ToLower());

            if (filter.OpenedFrom.HasValue)
                query = query.Where(a => a.CreatedAt >= filter.OpenedFrom);

            if (filter.OpenedTo.HasValue)
                query = query.Where(a => a.CreatedAt <= filter.OpenedTo);

            query = filter.SortBy?.ToLower() switch
            {
                "name" => filter.SortDesc ? query.OrderByDescending(a => a.Name) : query.OrderBy(a => a.Name),
                "code" => filter.SortDesc ? query.OrderByDescending(a => a.Code) : query.OrderBy(a => a.Code),
                "city" => filter.SortDesc ? query.OrderByDescending(a => a.City) : query.OrderBy(a => a.City),
                "country" => filter.SortDesc ? query.OrderByDescending(a => a.Country) : query.OrderBy(a => a.Country),
                _ => query.OrderBy(a => a.Name)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<Airport>(items, totalCount, filter.Page, filter.PageSize);
        }

        public async Task AddAsync(Airport airport)
        {
            await _dbContext.Airports.AddAsync(airport);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Airport airport)
        {
            _dbContext.Airports.Update(airport);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Airport airport)
        {
            _dbContext.Airports.Remove(airport);
            await _dbContext.SaveChangesAsync();
        }
    }
}

