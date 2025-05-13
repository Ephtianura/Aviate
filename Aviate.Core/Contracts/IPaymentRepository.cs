using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.DataAccess.Repositories
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task DeleteAsync(Payment payment);
        Task<Payment?> GetByIdAsync(Guid id);
        Task<PagedResult<Payment>> GetFilteredAsync(PaymentFilter filter);
        Task UpdateAsync(Payment payment);
    }
}