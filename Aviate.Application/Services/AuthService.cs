using Aviate.Application.Contracts;
using Aviate.Application.Dto;
using Aviate.Application.Exceptions;
using Aviate.Core.Contracts;
using Aviate.Core.Models;
using FluentValidation;

namespace Aviate.Application.Services
{
    //================== AUTH ==================
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IValidator<UserCreateDto> _createValidator;

        public AuthService(IUserRepository users, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, IValidator<UserCreateDto> createValidator)
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _createValidator = createValidator;
        }

        // Реєстрація
        public async Task RegisterAsync(UserCreateDto dto)
        {
            // Валідація
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Нормалізація ел. адреси
            var emailNormalized = dto.Email.Trim().ToLower();

            // Отримання ел. адреси 
            var existingByEmail = await _users.GetByEmailAsync(emailNormalized);
            if (existingByEmail != null)
                throw new EmailAlreadyExistsException(emailNormalized);

            // Генерація хеша
            var hashedPassword = _passwordHasher.Generate(dto.Password);

            // Створення користувача в БД            
            var user = User.Create(dto.FullName, emailNormalized, hashedPassword);

            // Збереження
            await _users.AddAsync(user);
        }

        // Вхід
        public async Task<string> Login(string email, string password)
        {
            // Нормалізація ел. адреси
            var emailNormalized = email.Trim().ToLower();

            // Отримання ел. адреси 
            var user = await _users.GetByEmailAsync(emailNormalized);

            // Перевірка логіна і пароля 
            if (user == null || !_passwordHasher.Verify(password, user.PasswordHash))
                throw new AuthenticationException();

            // Генерація JWT
            var token = _jwtProvider.GenerateToken(user);
            return token;
        }

        private string NormalizeEmail(string email) => email.Trim().ToLower();
    }
}
