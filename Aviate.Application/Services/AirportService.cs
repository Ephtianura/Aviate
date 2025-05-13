//using Aviate.Application.Contracts;
//using Aviate.Application.Dto;
//using Aviate.Application.Exceptions;
//using Aviate.Core.Contracts;
//using Aviate.Core.Filters;
//using Aviate.Core.Models;
//using Aviate.DataAccess.Repositories;
//using FluentValidation;

//namespace Aviate.Application.Services
//{
//    public class AirportService 
//    {
//        private readonly IAirportRepository _airports;
//        private readonly IValidator<AirportUpdateDto> _updateValidator;

//        public AirportService(
//            IAirportRepository airports,
//            IValidator<AirportUpdateDto> updateValidator
//        )
//        {
//            _airports = airports;
//            _updateValidator = updateValidator;
//        }

//        // Отримати аеропорт по ID
//        public async Task<Airport> GetByIdAsync(Guid id)
//        {
//            var airport = await _airports.GetByIdAsync(id);
//            if (airport == null)
//                throw new KeyNotFoundException($"Airport with id {id} not found.");
//            return airport;
//        }

//        // Отримати аеропорти за фільтром
//        public async Task<PagedResult<Airport>> GetFilteredAsync(AirportFilter filter)
//        {
//            var airports = await _airports.GetFilteredAsync(filter);
//            if (airports == null)
//                throw new KeyNotFoundException("Airports not found.");
//            return airports;
//        }

//        // Оновити профіль аеропорту
//        public async Task UpdateProfileAsync(Guid id, AirportUpdateDto dto)
//        {
//            var airport = await GetByIdAsync(id);

//            var validationResult = await _updateValidator.ValidateAsync(dto);
//            if (!validationResult.IsValid)
//                throw new ValidationException(validationResult.Errors);

//            if (!string.IsNullOrEmpty(dto.Name) && dto.Name != airport.Name)
//                airport.ChangeName(dto.Name);

//            if (!string.IsNullOrEmpty(dto.City) && dto.City != airport.City)
//                airport.ChangeCity(dto.City);

//            if (!string.IsNullOrEmpty(dto.Country) && dto.Country != airport.Country)
//                airport.ChangeCountry(dto.Country);

//            await _airports.UpdateAsync(airport);
//        }

//        // Видалити аеропорт
//        public async Task DeleteAsync(Guid id)
//        {
//            var airport = await _airports.GetByIdAsync(id);
//            if (airport == null)
//                throw new KeyNotFoundException($"Airport with id {id} not found.");

//            await _airports.DeleteAsync(airport);
//        }
//    }
//}
