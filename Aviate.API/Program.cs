using Aviate.API.Extensions;
using Aviate.API.Mapping;
using Aviate.API.Middleware.Exceptions;
using Aviate.Application.Contracts;
using Aviate.Application.Services;
using Aviate.Application.Validation.AirplaneValidator;
using Aviate.Application.Validation.AirportValidator;
using Aviate.Application.Validation.FlightValidator;
using Aviate.Application.Validation.UserValidator;
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
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AirplaneCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AirportCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<FlightCreateValidator>();

// Auto Mapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserProfile>();
    cfg.AddProfile<AirplaneProfile>();
    cfg.AddProfile<AirportProfile>();
    cfg.AddProfile<FlightProfile>();
    cfg.AddProfile<SeatProfile>();
});

    
var configuration = builder.Configuration;

builder.Services.AddDbContext<AviateDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(AviateDbContext)));
    });

// DI Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAirplaneRepository, AirplaneRepository>();
builder.Services.AddScoped<IAirportRepository, AirportRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();

// DI Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAirplaneService, AirplaneService>();
builder.Services.AddScoped<IAirportService, AirportService>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<ISeatService, SeatService>();

// DI Infrastructure
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Налаштування JWT токену
builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!;
builder.Services.AddApiAuthentication(jwtOptions);

// Swagger Comments
//builder.Services.AddSwaggerGen(c =>
//{
//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
//});

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
});

// Обробник помилок
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
