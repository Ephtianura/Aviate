//using Aviate.Application.Dto;
//using Aviate.Core.Contracts;
//using Aviate.Core.Filters;
//using Aviate.Core.Models;
//using Aviate.DataAccess.Repositories;
//using FluentValidation;

//namespace Aviate.Application.Services
//{
//    public class FlightService
//    {
//        private readonly IFlightRepository _flights;
//        private readonly IAirplaneRepository _airplanes;
//        private readonly IAirportRepository _airports;
//        private readonly IValidator<FlightRequest> _validator;
//        private readonly IValidator<FlightFilter> _filterValidator;

//        public FlightService(
//            IFlightRepository flights,
//            IAirplaneRepository airplanes,
//            IAirportRepository airports,
//            IValidator<FlightRequest> validator,
//            IValidator<FlightFilter> filterValidator)
//        {
//            _flights = flights;
//            _airplanes = airplanes;
//            _airports = airports;
//            _validator = validator;
//            _filterValidator = filterValidator;
//        }

//        // ===================== Get by Id =====================
//        public async Task<Flight> GetByIdAsync(Guid id)
//        {
//            var flight = await _flights.GetByIdAsync(id);
//            if (flight == null)
//                throw new KeyNotFoundException($"Flight with id {id} not found.");
//            return flight;
//        }

//        // ===================== Get Filtered =====================
//        public async Task<PagedResult<Flight>> GetFilteredAsync(FlightFilter filter)
//        {
//            var validationResult = await _filterValidator.ValidateAsync(filter);
//            if (!validationResult.IsValid)
//                throw new ValidationException(validationResult.Errors);

//            var flights = await _flights.GetFilteredAsync(filter);
//            if (flights == null)
//                throw new KeyNotFoundException("Flights not found.");
//            return flights;
//        }

//        // ===================== Create =====================
//        public async Task<Flight> CreateAsync(FlightRequest dto)
//        {
//            var validationResult = await _validator.ValidateAsync(dto);
//            if (!validationResult.IsValid)
//                throw new ValidationException(validationResult.Errors);

//            // Проверка уникальности номера рейса
//            var existing = await _flights.GetByFlightNumberAsync(dto.FlightNumber.Trim().ToUpperInvariant());
//            if (existing != null)
//                throw new ArgumentException($"Flight with number {dto.FlightNumber} already exists.");

//            var airplane = await _airplanes.GetByIdAsync(dto.AirplaneId);
//            if (airplane == null)
//                throw new KeyNotFoundException($"Airplane with id {dto.AirplaneId} not found.");

//            var departureAirport = await _airports.GetByIdAsync(dto.DepartureAirportId);
//            if (departureAirport == null)
//                throw new KeyNotFoundException($"Departure airport with id {dto.DepartureAirportId} not found.");

//            var arrivalAirport = await _airports.GetByIdAsync(dto.ArrivalAirportId);
//            if (arrivalAirport == null)
//                throw new KeyNotFoundException($"Arrival airport with id {dto.ArrivalAirportId} not found.");

//            var flight = Flight.Create(
//                airplane,
//                departureAirport,
//                arrivalAirport,
//                dto.FlightNumber,
//                dto.BasePrice,
//                dto.DepartureTime,
//                dto.ArrivalTime,
//                dto.EconomySeats,
//                dto.BusinessSeats,
//                dto.FirstClassSeats
//            );

//            if (dto.Status.HasValue)
//                flight.ChangeStatus(dto.Status.Value);

//            await _flights.AddAsync(flight);
//            return flight;
//        }

//        // ===================== Update =====================
//        public async Task UpdateAsync(Guid id, FlightRequest dto)
//        {
//            var flight = await GetByIdAsync(id);

//            var validationResult = await _validator.ValidateAsync(dto);
//            if (!validationResult.IsValid)
//                throw new ValidationException(validationResult.Errors);

//            // Изменяем номер рейса
//            if (!string.IsNullOrWhiteSpace(dto.FlightNumber) && dto.FlightNumber != flight.FlightNumber)
//            {
//                var existing = await _flights.GetByFlightNumberAsync(dto.FlightNumber.Trim().ToUpperInvariant());
//                if (existing != null && existing.Id != id)
//                    throw new ArgumentException($"Flight number {dto.FlightNumber} already exists.");
//                flight.ChangeFlightNumber(dto.FlightNumber.Trim().ToUpperInvariant());
//            }

//            // Изменяем цены и расписание
//            if (dto.BasePrice != flight.BasePrice)
//                flight.ChangeBasePrice(dto.BasePrice);

//            if (dto.DepartureTime != flight.DepartureTime || dto.ArrivalTime != flight.ArrivalTime)
//                flight.ChangeSchedule(dto.DepartureTime, dto.ArrivalTime);

//            // Изменяем статус
//            if (dto.Status.HasValue && dto.Status.Value != flight.Status)
//                flight.ChangeStatus(dto.Status.Value);

//            // Изменяем самолет
//            if (dto.AirplaneId != flight.AirplaneId)
//            {
//                var airplane = await _airplanes.GetByIdAsync(dto.AirplaneId);
//                if (airplane == null)
//                    throw new KeyNotFoundException($"Airplane with id {dto.AirplaneId} not found.");
//                flight.AssignAirplane(airplane);
//            }

//            // Изменяем аэропорты
//            if (dto.DepartureAirportId != flight.DepartureAirportId)
//            {
//                var departureAirport = await _airports.GetByIdAsync(dto.DepartureAirportId);
//                if (departureAirport == null)
//                    throw new KeyNotFoundException($"Departure airport with id {dto.DepartureAirportId} not found.");
//                flight.AssignDepartureAirport(departureAirport);
//            }

//            if (dto.ArrivalAirportId != flight.ArrivalAirportId)
//            {
//                var arrivalAirport = await _airports.GetByIdAsync(dto.ArrivalAirportId);
//                if (arrivalAirport == null)
//                    throw new KeyNotFoundException($"Arrival airport with id {dto.ArrivalAirportId} not found.");
//                flight.AssignArrivalAirport(arrivalAirport);
//            }

//            // Генерация мест
//            flight.GenerateSeats(dto.EconomySeats, dto.BusinessSeats, dto.FirstClassSeats);

//            await _flights.UpdateAsync(flight);
//        }

//        // ===================== Delete =====================
//        public async Task DeleteAsync(Guid id)
//        {
//            var flight = await GetByIdAsync(id);
//            await _flights.DeleteAsync(flight);
//        }
//    }
//}
