using Aviate.Core.Filters;
using Aviate.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Aviate.DataAccess.Repositories
{
    // ===================== BOOKING =====================
    public class BookingRepository : IBookingRepository
    {
        private readonly AviateDbContext _dbContext;
        public BookingRepository(AviateDbContext db) => _dbContext = db;

        public async Task<Booking?> GetByIdAsync(Guid id) =>
            await _dbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Flight)
                .Include(b => b.Seat)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<List<Booking>> GetByUserIdAsync(Guid userId) =>
            await _dbContext.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Flight)
                .Include(b => b.Seat)
                .ToListAsync();

        public async Task<PagedResult<Booking>> GetFilteredAsync(BookingFilter filter)
        {
            var query = _dbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Flight)
                .Include(b => b.Seat)
                .AsQueryable();

            if (filter.UserId.HasValue)
                query = query.Where(b => b.UserId == filter.UserId);

            if (filter.FlightId.HasValue)
                query = query.Where(b => b.FlightId == filter.FlightId);

            if (filter.Status.HasValue)
                query = query.Where(b => b.Status == filter.Status);

            if (filter.MinTotalPrice.HasValue)
                query = query.Where(b => b.TotalPrice >= filter.MinTotalPrice);

            if (filter.MaxTotalPrice.HasValue)
                query = query.Where(b => b.TotalPrice <= filter.MaxTotalPrice);

            if (filter.BookingFrom.HasValue)
                query = query.Where(b => b.BookingDate >= filter.BookingFrom);

            if (filter.BookingTo.HasValue)
                query = query.Where(b => b.BookingDate <= filter.BookingTo);

            query = filter.SortBy?.ToLower() switch
            {
                "bookingdate" => filter.SortDesc ? query.OrderByDescending(b => b.BookingDate) : query.OrderBy(b => b.BookingDate),
                "totalprice" => filter.SortDesc ? query.OrderByDescending(b => b.TotalPrice) : query.OrderBy(b => b.TotalPrice),
                "status" => filter.SortDesc ? query.OrderByDescending(b => b.Status) : query.OrderBy(b => b.Status),
                _ => query.OrderBy(b => b.BookingDate)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<Booking>(items, totalCount, filter.Page, filter.PageSize);
        }


        public async Task AddAsync(Booking booking)
        {
            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Booking booking)
        {
            _dbContext.Bookings.Update(booking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Booking booking)
        {
            _dbContext.Bookings.Remove(booking);
            await _dbContext.SaveChangesAsync();
        }
    }
}

