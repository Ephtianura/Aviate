using Aviate.Application.Contracts;
using Aviate.Application.Dto.Flight;
using Aviate.Core.Models;
using Aviate.DataAccess.Repositories;

namespace Aviate.Application.Services
{
    /// <summary>
    /// УВАГА: ЦЕЙ СЕРВІС СТВОРЕНИЙ **СУТО** ДЛЯ ТЕСТУ ШВИДКОГО НАПОВНЕННЯ БД.
    /// НЕ ВИКОРИСТОВУВАТИ БІЛЬШЕ ОДНОГО РАЗУ, ЩОБ НЕ НАСЛАЮВАЛИСЯ ДАНІ ОДНІ НА ОДНОГО
    /// </summary>
    public class DegenerateService(IAirplaneRepository airplanes, IAirportRepository airports, IFlightService flightService) : IDegenerateService
    {
        private static bool _alreadyRun = false;


        private readonly IAirplaneRepository _airplanes = airplanes;
        private readonly IAirportRepository _airports = airports;
        private readonly IFlightService _flights = flightService;
        public async Task GenerateRandomFlightsAsync()
        {
            if (_alreadyRun)
                return;

            _alreadyRun = true;

            var airplanes = (await _airplanes.GetFilteredAsync(new Core.Filters.AirplaneFilter
            {
                Page = 1,
                PageSize = 99999
            })).Items.ToList();

            var airports = (await _airports.GetFilteredAsync(new Core.Filters.AirportFilter
            {
                Country = "Ukraine",
                Page = 1,
                PageSize = 99999
            })).Items.ToList();

            var random = new Random();

            var start = DateTimeOffset.UtcNow;
            var end = start.AddMonths(1);

            var dtos = new List<FlightCreateDto>();

            while (start < end)
            {
                foreach (var dep in airports)
                    foreach (var arr in airports)
                    {
                        if (dep.Id == arr.Id)
                            continue;

                        var flightsToday = random.Next(0, 4); //0-4 в течении дня

                        for (int i = 0; i < flightsToday; i++)
                        {
                            var airplane = airplanes[random.Next(airplanes.Count)];

                            var departureTime = start.AddHours(random.Next(6, 22));
                            var arrivalTime = departureTime.AddHours(random.Next(1, 3));

                            var capacity = airplane.Capacity;

                            var economy = (int)(capacity * 0.7); // 70%
                            var business = (int)(capacity * 0.2);// 20%
                            var first = capacity - economy - business;

                            dtos.Add(new FlightCreateDto(
                                airplane.Id,
                                dep.Id,
                                arr.Id,
                                random.Next(100, 2000), // Ціна 
                                departureTime,
                                arrivalTime,
                                economy,
                                business,
                                first
                            ));
                        }
                    }

                start = start.AddDays(1);
            }

            await _flights.CreateBatchAsync(dtos);
        }
    }
}
