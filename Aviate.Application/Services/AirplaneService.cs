using Aviate.Application.Contracts;
using Aviate.Application.Dto;
using Aviate.Application.Exceptions;
using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;
using Aviate.DataAccess.Repositories;
using FluentValidation;

namespace Aviate.Application.Services
{
    public class AirplaneService : IAirplaneService
    {
        private readonly IAirplaneRepository _airplanes;
        private readonly IValidator<AirplaneRequest> _createValidator;
        private readonly IValidator<AirplaneUpdateDto> _updateValidator;
        private readonly IValidator<AirplaneFilter> _filterValidator;


        public AirplaneService(
            IAirplaneRepository airplanes,
            IValidator<AirplaneRequest> createValidator,
            IValidator<AirplaneFilter> filterValidator,
            IValidator<AirplaneUpdateDto> updateValidator)
        {
            _airplanes = airplanes;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _filterValidator = filterValidator;

        }

        // Отримати літак по ID
        public async Task<Airplane> GetByIdAsync(Guid id)
        {
            var airplane = await _airplanes.GetByIdAsync(id);
            if (airplane == null)
                throw new EntityNotFoundException("Airplanes", id);
            return airplane;
        }

        // Отримати літаки за фільтром
        public async Task<PagedResult<Airplane>> GetFilteredAsync(AirplaneFilter filter)
        {
            // Валідація фільтра
            var validationResult = await _filterValidator.ValidateAsync(filter);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var airplanes = await _airplanes.GetFilteredAsync(filter);
            if (airplanes == null)
                throw new EntityNotFoundException("Airplanes");
            return airplanes;
        }

        // Створити літак
        public async Task<Airplane> CreateAsync(AirplaneRequest request)
        {
            // Валідація запиту
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Перевірка чи нема такого самого літака
            var existing = await _airplanes.GetByRegistrationAsync(request.RegistrationNumber.Trim().ToUpperInvariant());
            if (existing != null)
                throw new EntityAlreadyExistsException("Airplane", request.RegistrationNumber);

            // Створення літака
            var airplane = Airplane.Create(
                request.Model,
                request.RegistrationNumber,
                request.Capacity,
                request.ManufactureDate
            );

            // Якщо встановлено поле Status - міняємо
            if (request.Status.HasValue)
                airplane.ChangeStatus(request.Status.Value);

            await _airplanes.AddAsync(airplane);
            return airplane;
        }

        // Оновити літак
        public async Task UpdateAsync(Guid id, AirplaneUpdateDto request)
        {
            // Отримати літак
            var airplane = await GetByIdAsync(id);

            // Валідація запиту
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Якщо передали значення - встановлюємо
            if (!string.IsNullOrEmpty(request.RegistrationNumber) && request.RegistrationNumber != airplane.RegistrationNumber)
            {
                var existing = await _airplanes.GetByRegistrationAsync(request.RegistrationNumber.Trim().ToUpperInvariant());
                if (existing != null && existing.Id != id)
                    throw new EntityAlreadyExistsException("Airplane", request.RegistrationNumber);

                airplane.ChangeRegistrationNumber(request.RegistrationNumber.Trim().ToUpperInvariant());
            }
            

            if (!string.IsNullOrEmpty(request.Model) && request.Model != airplane.Model)
                airplane.ChangeModel(request.Model);

            if (request.Capacity.HasValue && request.Capacity.Value != airplane.Capacity)
                airplane.ChangeCapacity(request.Capacity.Value);

            if (request.Status.HasValue && request.Status.Value != airplane.Status)
                airplane.ChangeStatus(request.Status.Value);

            if (request.ManufactureDate.HasValue && request.ManufactureDate.Value != airplane.ManufactureDate)
                airplane.ChangeManufactureDate(request.ManufactureDate.Value);

            await _airplanes.UpdateAsync(airplane);
        }

        // Видалити літак
        public async Task DeleteAsync(Guid id)
        {
            var airplane = await GetByIdAsync(id);
            await _airplanes.DeleteAsync(airplane);
        }
    }
}
