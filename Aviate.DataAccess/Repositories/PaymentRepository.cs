using Aviate.Core.Filters;
using Aviate.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Aviate.DataAccess.Repositories
{
    // ===================== PAYMENT =====================
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AviateDbContext _dbContext;
        public PaymentRepository(AviateDbContext db) => _dbContext = db;

        public async Task<Payment?> GetByIdAsync(Guid id) =>
            await _dbContext.Payments
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(p => p.Id == id);
        public async Task<List<Payment>> GetByBookingIdAsync(Guid bookingId) =>
            await _dbContext.Payments
            .Where(p => p.BookingId == bookingId)
            .ToListAsync();
        

        public async Task<PagedResult<Payment>> GetFilteredAsync(PaymentFilter filter)
        {
            var query = _dbContext.Payments
                .Include(p => p.Booking)
                .AsQueryable();

            if (filter.BookingId.HasValue)
                query = query.Where(p => p.BookingId == filter.BookingId);

            if (filter.Status.HasValue)
                query = query.Where(p => p.Status == filter.Status);

            if (filter.Method.HasValue)
                query = query.Where(p => p.Method == filter.Method);

            if (filter.MinAmount.HasValue)
                query = query.Where(p => p.Amount >= filter.MinAmount);

            if (filter.MaxAmount.HasValue)
                query = query.Where(p => p.Amount <= filter.MaxAmount);

            if (filter.CreatedFrom.HasValue)
                query = query.Where(p => p.CreatedAt >= filter.CreatedFrom);

            if (filter.CreatedTo.HasValue)
                query = query.Where(p => p.CreatedAt <= filter.CreatedTo);

            query = filter.SortBy?.ToLower() switch
            {
                "createdat" => filter.SortDesc ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                "amount" => filter.SortDesc ? query.OrderByDescending(p => p.Amount) : query.OrderBy(p => p.Amount),
                "status" => filter.SortDesc ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status),
                "method" => filter.SortDesc ? query.OrderByDescending(p => p.Method) : query.OrderBy(p => p.Method),
                _ => query.OrderBy(p => p.CreatedAt)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<Payment>(items, totalCount, filter.Page, filter.PageSize);
        }


        public async Task AddAsync(Payment payment)
        {
            await _dbContext.Payments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _dbContext.Payments.Update(payment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Payment payment)
        {
            _dbContext.Payments.Remove(payment);
            await _dbContext.SaveChangesAsync();
        }
    }
}

