using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Aviate.DataAccess.Repositories
{
    // ===================== AIRPLANE =====================
    public class AirplaneRepository : IAirplaneRepository
    {
        private readonly AviateDbContext _dbContext;
        public AirplaneRepository(AviateDbContext db) => _dbContext = db;

        public async Task<Airplane?> GetByIdAsync(Guid id) =>
            await _dbContext.Airplanes.FindAsync(id);

        public async Task<Airplane?> GetByRegistrationAsync(string registrationNumber)
        {
            if (string.IsNullOrWhiteSpace(registrationNumber))
                return null;

            var normalized = registrationNumber.Trim().ToUpperInvariant();
            return await _dbContext.Airplanes
                .FirstOrDefaultAsync(a => a.RegistrationNumber == normalized);
        }

        public async Task<PagedResult<Airplane>> GetFilteredAsync(AirplaneFilter filter)
        {
            var query = _dbContext.Airplanes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim().ToLower();
                query = query.Where(a =>
                    a.Model.ToLower().Contains(term) ||
                    a.RegistrationNumber.ToLower().Contains(term));
            }

            if (filter.Status.HasValue)
                query = query.Where(a => a.Status == filter.Status);

            if (filter.MinCapacity.HasValue)
                query = query.Where(a => a.Capacity >= filter.MinCapacity);

            if (filter.MaxCapacity.HasValue)
                query = query.Where(a => a.Capacity <= filter.MaxCapacity);

            if (filter.ManufactureFrom.HasValue)
                query = query.Where(a => a.ManufactureDate >= filter.ManufactureFrom);

            if (filter.ManufactureTo.HasValue)
                query = query.Where(a => a.ManufactureDate <= filter.ManufactureTo);

            query = filter.SortBy?.ToLower() switch
            {
                "model" => filter.SortDesc ? query.OrderByDescending(a => a.Model) : query.OrderBy(a => a.Model),
                "registrationnumber" => filter.SortDesc ? query.OrderByDescending(a => a.RegistrationNumber) : query.OrderBy(a => a.RegistrationNumber),
                "capacity" => filter.SortDesc ? query.OrderByDescending(a => a.Capacity) : query.OrderBy(a => a.Capacity),
                "manufacturedate" => filter.SortDesc ? query.OrderByDescending(a => a.ManufactureDate) : query.OrderBy(a => a.ManufactureDate),
                _ => query.OrderBy(a => a.Model)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<Airplane>(items, totalCount, filter.Page, filter.PageSize);
        }


        public async Task AddAsync(Airplane airplane)
        {
            await _dbContext.Airplanes.AddAsync(airplane);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Airplane airplane)
        {
            _dbContext.Airplanes.Update(airplane);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Airplane airplane)
        {
            _dbContext.Airplanes.Remove(airplane);
            await _dbContext.SaveChangesAsync();
        }
    }
}

