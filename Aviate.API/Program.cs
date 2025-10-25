using Aviate.API.Extensions;
using Aviate.API.Mapping;
using Aviate.API.Middleware.Exceptions;
using Aviate.Application.Contracts;
using Aviate.Application.Services;
using Aviate.Application.Validation.AirplaneValidator;
using Aviate.Application.Validation.AirportValidator;
using Aviate.Application.Validation.BookingValidator;
using Aviate.Application.Validation.FlightValidator;
using Aviate.Application.Validation.UserValidator;
using Aviate.Core.Contracts;
using Aviate.DataAccess;
using Aviate.DataAccess.Repositories;
using Aviate.Infrastructure.Auth;
using Aviate.Infrastructure.Payment;
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
builder.Services.AddValidatorsFromAssemblyContaining<BookingCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PaymentFilterValidator>();
// Auto Mapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserProfile>();
    cfg.AddProfile<AirplaneProfile>();
    cfg.AddProfile<AirportProfile>();
    cfg.AddProfile<FlightProfile>();
    cfg.AddProfile<SeatProfile>();
    cfg.AddProfile<BookingProfile>();
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
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// DI Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAirplaneService, AirplaneService>();
builder.Services.AddScoped<IAirportService, AirportService>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IDegenerateService, DegenerateService>();

// DI Infrastructure
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IPaymentGatewayMock, PaymentGatewayMock>();

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:3004")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());

});



// Створення додатку
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AviateDbContext>();
    db.Database.Migrate();
}




// Свагер
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

// Параметри кукі
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
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
