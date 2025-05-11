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
        private readonly IValidator<UserUpdateAdminDto> _adminUpdateValidator;

        public UserService
        (
            IUserRepository users,
            IPasswordHasher passwordHasher,
            IValidator<UserUpdateDto> updateValidator,
            IValidator<UserUpdateAdminDto> adminUpdateValidator
        )
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _updateValidator = updateValidator;
            _adminUpdateValidator = adminUpdateValidator;
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
        public async Task<PagedResult<User>> GetFilteredAsync(UserFilter filter)
        {            
            var users = await _users.GetFilteredAsync(filter);
            if (users == null)
                throw new KeyNotFoundException($"Users not found.");
            return users;
        }

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

        public async Task UserUpdateByAdminAsync(Guid id, UserUpdateAdminDto dto)
        {
            var user = await GetByIdAsync(id);

            var validationResult = await _adminUpdateValidator.ValidateAsync(dto);
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

            if (!string.IsNullOrEmpty(dto.Phone) && dto.Phone != user.Phone)
                user.ChangePhone(dto.Phone);

            if (dto.Role.HasValue && dto.Role != user.Role)
                user.ChangeRole(dto.Role.Value);

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
    }
}
