using Aviate.Application.Dto.Flight;
using Aviate.Application.Exceptions;
using Aviate.Core.Filters;
using Aviate.Core.Models;
using Aviate.DataAccess.Repositories;
using FluentValidation;
using System.Security.Cryptography;
using System.Text;

namespace Aviate.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flights;
        private readonly IAirplaneRepository _airplanes;
        private readonly IAirportRepository _airports;
        private readonly ISeatRepository _seats;
        private readonly IValidator<FlightCreateDto> _createValidator;
        private readonly IValidator<FlightUpdateDto> _updateValidator;
        private readonly IValidator<FlightFilter> _filterValidator;

        public FlightService
            (
            IFlightRepository flights,
            IAirplaneRepository airplanes,
            IAirportRepository airports,
            ISeatRepository seats,
            IValidator<FlightCreateDto> createValidator,
            IValidator<FlightUpdateDto> updateValidator,
            IValidator<FlightFilter> filterValidator
            )
        {
            _flights = flights;
            _airplanes = airplanes;
            _airports = airports;
            _seats = seats;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _filterValidator = filterValidator;

        }

        // Отримати рейс по ID
        public async Task<Flight> GetByIdAsync(Guid id)
        {
            return await GetFlightByIdAsync(id); ;
        }

        // Отримати рейси за фільтром
        public async Task<PagedResult<Flight>> GetFilteredAsync(FlightFilter filter)
        {
            // Валідація фільтра
            await _filterValidator.ValidateAndThrowAsync(filter);

            return await _flights.GetFilteredAsync(filter);
        }

        // Створити рейс
        public async Task<Flight> CreateAsync(FlightCreateDto request)
        {
            // Валідація запиту
            await _createValidator.ValidateAndThrowAsync(request);

            var airplane = await _airplanes.GetByIdAsync(request.AirplaneId);
            if (airplane is null)
                throw new EntityNotFoundException("Airplane", request.AirplaneId);

            var totalSeats = request.EconomySeats + request.BusinessSeats + request.FirstClassSeats;

            if (airplane.Capacity < totalSeats)
                throw new ArgumentException("Total number of seats exceeds airplane capacity");


            // Перевірка чи не занятий літак
            var airplaneConflict = await _flights.ExistsForAirplaneAtTimeAsync(request.AirplaneId, request.DepartureTime);
            if (airplaneConflict)
                throw new FlightConflictException("This airplane already has a flight at the same departure time.");

            // Перевірка чи нема такого самого рейса
            var routeConflict = await _flights.ExistsAsync(
                    request.AirplaneId,
                    request.DepartureAirportId,
                    request.ArrivalAirportId,
                    request.DepartureTime
                );
            if (routeConflict)
                throw new FlightConflictException("A flight for this route already exists at the same time.");


            var flightNumber = GenerateFlightNumber(
            request.AirplaneId,
            request.DepartureAirportId,
            request.ArrivalAirportId,
            request.DepartureTime);

            var departureAirport = await _airports.GetByIdAsync(request.DepartureAirportId);
            if (departureAirport is null)
                throw new EntityNotFoundException("Airport", request.DepartureAirportId);

            var arrivalAirport = await _airports.GetByIdAsync(request.ArrivalAirportId);
            if (arrivalAirport is null)
                throw new EntityNotFoundException("Airport", request.ArrivalAirportId);


            // Створення рейсу (автоматичне заповнення місць у домені)
            var flight = Flight.Create(

                airplane,
                departureAirport,
                arrivalAirport,

                flightNumber,
                request.BasePrice,

                request.DepartureTime,
                request.ArrivalTime,

                request.EconomySeats,
                request.BusinessSeats,
                request.FirstClassSeats
            );

            await _flights.AddAsync(flight);

            return flight;
        }

        public async Task CreateBatchAsync(List<FlightCreateDto> requests)
        {
            var airplanesResult = await _airplanes.GetFilteredAsync(new Core.Filters.AirplaneFilter()
            {
                Page = 1,
                PageSize = 99999
            });
            var airplanes = airplanesResult.Items;

            var airportsResult = await _airports.GetFilteredAsync(new Core.Filters.AirportFilter()
            {
                Page = 1,
                PageSize = 99999
            });

            var airports = airportsResult.Items;

            var airplaneMap = airplanes.ToDictionary(x => x.Id);
            var airportMap = airports.ToDictionary(x => x.Id);

            var flights = new List<Flight>();

            foreach (var r in requests)
            {
                var airplane = airplaneMap[r.AirplaneId];
                var dep = airportMap[r.DepartureAirportId];
                var arr = airportMap[r.ArrivalAirportId];

                var totalSeats = r.EconomySeats + r.BusinessSeats + r.FirstClassSeats;

                if (airplane.Capacity < totalSeats)
                    continue;

                flights.Add(CreateFast(
                    airplane,
                    dep,
                    arr,
                    r.BasePrice,
                    r.DepartureTime,
                    r.ArrivalTime,
                    r.EconomySeats,
                    r.BusinessSeats,
                    r.FirstClassSeats
                ));
            }

            await _flights.AddRangeFastAsync(flights);
        }

        public Flight CreateFast(
            Airplane airplane,
            Airport departure,
            Airport arrival,
            decimal basePrice,
            DateTimeOffset departureTime,
            DateTimeOffset arrivalTime,
            int economy,
            int business,
            int first)
        {
             int _counter = 0;
        var flightNumber = GenerateFlightNumber(
                airplane.Id,
                departure.Id,
                arrival.Id,
                departureTime);

            return Flight.Create(
                airplane,
                departure,
                arrival,
                flightNumber,
                basePrice,
                departureTime,
                arrivalTime,
                economy,
                business,
                first
            );
        }

        // Оновити рейс
        public async Task UpdateAsync(Guid id, FlightUpdateDto request)
        {
            // Валідація запиту
            await _updateValidator.ValidateAndThrowAsync(request);

            // Отримання рейсу
            var flight = await GetFlightByIdAsync(id);


            // Що передано - міняємо
            if (request.AirplaneId.HasValue && request.AirplaneId.Value != flight.AirplaneId)
            {
                var airplane = await _airplanes.GetByIdAsync(request.AirplaneId.Value);

                if (airplane is null)
                    throw new EntityNotFoundException("Airplane", request.AirplaneId.Value);

                flight.AssignAirplane(airplane);
            }

            if (request.DepartureAirportId.HasValue && request.DepartureAirportId.Value != flight.DepartureAirportId)
            {
                var departureAirport = await _airports.GetByIdAsync(request.DepartureAirportId.Value);

                if (departureAirport is null)
                    throw new EntityNotFoundException("Airport", request.DepartureAirportId.Value);

                flight.AssignDepartureAirport(departureAirport);
            }

            if (request.ArrivalAirportId.HasValue && request.ArrivalAirportId.Value != flight.ArrivalAirportId)
            {
                var arrivalAirport = await _airports.GetByIdAsync(request.ArrivalAirportId.Value);

                if (arrivalAirport is null)
                    throw new EntityNotFoundException("Airport", request.ArrivalAirportId.Value);

                flight.AssignArrivalAirport(arrivalAirport);
            }

            if (request.BasePrice.HasValue && request.BasePrice.Value != flight.BasePrice)
                flight.ChangeBasePrice(request.BasePrice.Value);

            if (request.Status.HasValue && request.Status.Value != flight.Status)
                flight.ChangeStatus(request.Status.Value);

            if (request.DepartureTime.HasValue && request.ArrivalTime.HasValue)
                flight.ChangeSchedule(request.DepartureTime.Value, request.ArrivalTime.Value);

            if (request.AirplaneId.HasValue && request.DepartureTime.HasValue)
            {
                var airplaneConflict = await _flights.ExistsForAirplaneAtTimeAsync(request.AirplaneId.Value, request.DepartureTime.Value);
                if (airplaneConflict)
                    throw new FlightConflictException("This airplane already has a flight at the same departure time.");
            }

            // Перевірка чи нема такого самого рейса
            if (request.AirplaneId.HasValue && request.DepartureAirportId.HasValue &&
                request.ArrivalAirportId.HasValue && request.DepartureTime.HasValue)
            {
                var routeConflict = await _flights.ExistsAsync(request.AirplaneId.Value, request.DepartureAirportId.Value,
                    request.ArrivalAirportId.Value, request.DepartureTime.Value);

                if (routeConflict)
                    throw new FlightConflictException("A flight for this route already exists at the same time.");
            }

            // Оновлюємо
            await _flights.UpdateAsync(flight);
        }

        // Видалити рейс
        public async Task DeleteAsync(Guid id)
        {
            var flight = await GetFlightByIdAsync(id);
            await _flights.DeleteAsync(flight);
        }

        // Отримати рейс
        private async Task<Flight> GetFlightByIdAsync(Guid id)
        {
            var flight = await _flights.GetByIdAsync(id);
            if (flight == null)
                throw new EntityNotFoundException("Flight", id);
            return flight;
        }

        // Генерація випадкового номера рейсу
        private string GenerateFlightNumber
            (
            Guid airplaneId,
            Guid departureAirportId,
            Guid arrivalAirportId,
            DateTimeOffset departureTime,
            string airlineCode = "AV"
            )
        {
            airlineCode = airlineCode.ToUpperInvariant();

            var input = $"{airlineCode}-{airplaneId}-{departureAirportId}-{arrivalAirportId}-{departureTime:yyyyMMddHHmm}-{departureTime:O}-{Guid.NewGuid():N}";

            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));

            var number = BitConverter.ToUInt32(hash, 0);
            var numberPart = number.ToString("D10");

            return $"{airlineCode}{numberPart}";
        }

    }
}
