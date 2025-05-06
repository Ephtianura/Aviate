using Aviate.Core.Contracts;
using Aviate.DataAccess;
using Aviate.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

//InitialDB
var configuration = builder.Configuration;

builder.Services.AddDbContext<AviateDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(AviateDbContext)));
    });

//DI Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();





var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
