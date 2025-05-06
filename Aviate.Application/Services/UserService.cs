using Aviate.Application.Contracts;
using Aviate.Application.Dto;
using Aviate.Application.Exceptions;
using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;
using FluentValidation;

namespace Aviate.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _users;
        private readonly IValidator<UserUpdateDto> _updateValidator;
        private readonly IPasswordHasher _passwordHasher;

        public UserService
        (
            IUserRepository users,
            IValidator<UserUpdateDto> updateValidator,
            IPasswordHasher passwordHasher
        )
        {
            _users = users;
            _updateValidator = updateValidator;
            _passwordHasher = passwordHasher;
        }

        // Отримати користувача по ID
        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _users.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with id {id} not found.");
            return user;
        }
        // Отримати користувачів за філтрами
        public async Task<PagedResult<User>> GetFilteredAsync(UserFilter filter) =>
            await _users.GetFilteredAsync(filter);



        // Оновити профіль користувача
        public async Task UpdateProfileAsync(Guid id, UserUpdateDto dto)
        {
            var user = await GetByIdAsync(id);


            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (!string.IsNullOrEmpty(dto.FullName) && dto.FullName != user.FullName)
                user.ChangeFullName(dto.FullName);

            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
            {
                var existing = await _users.GetByEmailAsync(dto.Email.Trim().ToLower());
                if (existing != null && existing.Id != id)
                    throw new EmailAlreadyExistsException(dto.Email);

                user.ChangeEmail(dto.Email.Trim().ToLower());
            }

            if (!string.IsNullOrEmpty(dto.Password))
                user.ChangePassword(_passwordHasher.Generate(dto.Password));

            if (!string.IsNullOrEmpty(dto.Phone) && dto.Phone != user.Phone)
                user.ChangePhone(dto.Phone);

            await _users.UpdateAsync(user);
        }




        // Видалити користувача
        public async Task DeleteAsync(Guid id)
        {
            var user = await _users.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with id {id} not found.");

            await _users.DeleteAsync(user);
        }

        private string NormalizeEmail(string email) => email.Trim().ToLower();

    }
}
