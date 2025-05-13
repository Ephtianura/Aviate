using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Aviate.DataAccess.Repositories
{
    // ===================== SEAT =====================
    public class SeatRepository : ISeatRepository
    {
        private readonly AviateDbContext _dbContext;
        public SeatRepository(AviateDbContext db) => _dbContext = db;

        public async Task<Seat?> GetByIdAsync(Guid id) =>
            await _dbContext.Seats.FindAsync(id);

        public async Task<PagedResult<Seat>> GetFilteredAsync(SeatFilter filter)
        {
            var query = _dbContext.Seats.AsQueryable();

            if (filter.FlightId.HasValue)
                query = query.Where(s => s.FlightId == filter.FlightId);

            if (filter.Class.HasValue)
                query = query.Where(s => s.Class == filter.Class);

            if (filter.IsBooked.HasValue)
                query = query.Where(s => s.IsBooked == filter.IsBooked);

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim().ToLower();
                query = query.Where(s => s.SeatNumber.ToLower().Contains(term));
            }

            query = filter.SortBy?.ToLower() switch
            {
                "seatnumber" => filter.SortDesc ? query.OrderByDescending(s => s.SeatNumber) : query.OrderBy(s => s.SeatNumber),
                "class" => filter.SortDesc ? query.OrderByDescending(s => s.Class) : query.OrderBy(s => s.Class),
                "isbooked" => filter.SortDesc ? query.OrderByDescending(s => s.IsBooked) : query.OrderBy(s => s.IsBooked),
                _ => query.OrderBy(s => s.SeatNumber)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<Seat>(items, totalCount, filter.Page, filter.PageSize);
        }


        public async Task AddAsync(Seat seat)
        {
            await _dbContext.Seats.AddAsync(seat);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seat seat)
        {
            _dbContext.Seats.Update(seat);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Seat seat)
        {
            _dbContext.Seats.Remove(seat);
            await _dbContext.SaveChangesAsync();
        }
    }
}

