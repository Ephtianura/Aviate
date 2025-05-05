//using Aviate.Core.Contracts;
//using Aviate.Core.Models;
//using Aviate.Core.Models.Common;
//using Aviate.Core.Models.Filters;
//using Microsoft.EntityFrameworkCore;

//namespace Aviate.DataAccess.Repositories
//{
//    public class UserRepository : IUserRepository
//    {
//        private readonly AutoRentalDbContext _db;

//        public UserRepository(AutoRentalDbContext db) => _db = db;

//        public async Task<User?> GetByIdAsync(int id) =>
//            await _db.Users.FindAsync(id);

//        public async Task<User?> GetByEmailAsync(string email)
//        {
//            var normalizedEmail = email.ToLower();
//            return await _db.Users
//                .AsNoTracking()
//                .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);
//        }



//        public async Task<PagedResult<User>> GetFilteredAsync(UserFilter filter, PagedRequest request)
//        {
//            IQueryable<User> query = _db.Users;

//            if (!string.IsNullOrWhiteSpace(filter.UserName))
//                query = query.Where(u => u.UserName.Contains(filter.UserName));
//            if (!string.IsNullOrWhiteSpace(filter.Email))
//                query = query.Where(u => u.Email.Contains(filter.Email));
//            if (filter.Status.HasValue)
//                query = query.Where(u => u.Status == filter.Status.Value);
//            if (filter.Role.HasValue)
//                query = query.Where(u => u.Role == filter.Role.Value);
//            if (filter.RegisteredFrom.HasValue)
//                query = query.Where(u => u.RegistrationDate >= filter.RegisteredFrom.Value);
//            if (filter.RegisteredTo.HasValue)
//                query = query.Where(u => u.RegistrationDate <= filter.RegisteredTo.Value);

//            return await query.PaginateAsync(request);
//        }

//        public async Task AddAsync(User user)
//        {

//            await _db.Users.AddAsync(user);
//            await _db.SaveChangesAsync();
//        }

//        public async Task UpdateAsync(User user)
//        {
//            _db.Users.Update(user);
//            await _db.SaveChangesAsync();
//        }

//        public async Task BlockUserAsync(int id)
//        {
//            var user = await _db.Users.FindAsync(id);
//            if (user != null)
//            {
//                user.Block();
//                await _db.SaveChangesAsync();
//            }
//        }

//        public async Task UnblockUserAsync(int id)
//        {
//            var user = await _db.Users.FindAsync(id);
//            if (user != null)
//            {
//                user.Unblock();
//                await _db.SaveChangesAsync();
//            }
//        }
//    }
//}
