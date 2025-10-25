using Aviate.Application.Contracts;
using Aviate.Application.Dto.Airport;
using Aviate.Application.Exceptions;
using Aviate.Core.Filters;
using Aviate.Core.Models;
using Aviate.DataAccess.Repositories;
using FluentValidation;

namespace Aviate.Application.Services
{
    public class AirportService : IAirportService
    {
        private readonly IAirportRepository _airports;
        private readonly IValidator<AirportCreateDto> _createValidator;
        private readonly IValidator<AirportUpdateDto> _updateValidator;
        private readonly IValidator<AirportFilter> _filterValidator;

        public AirportService
            (
            IAirportRepository airports,
            IValidator<AirportCreateDto> createValidator,
            IValidator<AirportUpdateDto> updateValidator,
            IValidator<AirportFilter> filterValidator
            )
        {
            _airports = airports;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _filterValidator = filterValidator;

        }

        // Отримати аеропорт по ID
        public async Task<Airport> GetByIdAsync(Guid id)
        {
            return await GetAirportByIdAsync(id);
        }

        // Отримати аеропорти за фільтром
        public async Task<PagedResult<Airport>> GetFilteredAsync(AirportFilter filter)
        {
            // Валідація фільтра
            await _filterValidator.ValidateAndThrowAsync(filter);

            return await _airports.GetFilteredAsync(filter);
        }

        // Створити аеропорт
        public async Task<Airport> CreateAsync(AirportCreateDto request)
        {
            // Валідація запиту
            await _createValidator.ValidateAndThrowAsync(request);

            // Перевірка чи нема такого самого аеропорта
            var existing = await _airports.GetByCode(request.Code.Trim().ToUpperInvariant());
            if (existing != null)
                return existing;
                //throw new EntityAlreadyExistsException("Airport", request.Code);

                // Створення літака
            var airport = Airport.Create(
                request.Name,
                request.Code,
                request.Country,
                request.City                    
            );

            await _airports.AddAsync(airport);
            return airport;
        }

        // Оновити аеропорт
        public async Task UpdateAsync(Guid id, AirportUpdateDto request)
        {
            // Валідація запиту
            await _updateValidator.ValidateAndThrowAsync(request);

            // Отримання аеропорту
            var airport = await GetAirportByIdAsync(id);

            // Що передано - міняємо
            if (!string.IsNullOrEmpty(request.Name) && request.Name != airport.Name)
                airport.ChangeName(request.Name);
            if (!string.IsNullOrEmpty(request.City) && request.City != airport.City)
                airport.ChangeCity(request.City);
            if (!string.IsNullOrEmpty(request.Country) && request.Country != airport.Country)
                airport.ChangeCountry(request.Country);

            // Оновлюємо
            await _airports.UpdateAsync(airport);
        }

        // Видалити аеропорт
        public async Task DeleteAsync(Guid id)
        {
            var airport = await GetAirportByIdAsync(id);
            await _airports.DeleteAsync(airport);
        }

        // Отримати аеропорт
        private async Task<Airport> GetAirportByIdAsync(Guid id)
        {
            var airport = await _airports.GetByIdAsync(id);
            if (airport == null)
                throw new EntityNotFoundException("Airport", id);
            return airport;
        }
    }
}
