using AutoMapper;
using Aviate.API.Extensions;
using Aviate.API.Mapping;
using Aviate.API.Middleware.Exceptions;
using Aviate.Application.Contracts;
using Aviate.Application.Services;
using Aviate.Application.Validation;
using Aviate.Core.Contracts;
using Aviate.DataAccess;
using Aviate.DataAccess.Repositories;
using Aviate.Infrastructure.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterValidator>();
builder.Services.AddFluentValidationAutoValidation()           // автоматическая валидация
    .AddFluentValidationClientsideAdapters();      // поддержка клиентской валидации (если нужно для фронта)

builder.Services.AddValidatorsFromAssemblyContaining<UserFilterValidator>();

// Auto Mapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserProfile>();
});

// InitialDB
var configuration = builder.Configuration;

builder.Services.AddDbContext<AviateDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(AviateDbContext)));
    });

// DI Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// DI Services
builder.Services.AddScoped<IUserService, UserService>();

// DI Auth
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Налаштування JWT токену
builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!;
builder.Services.AddApiAuthentication(jwtOptions);



// Створення додатку
var app = builder.Build();

// Свагер
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Параметри кукі
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
}
);

// Обробник помилок
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
