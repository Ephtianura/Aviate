
using Aviate.Application.Contracts;
using Aviate.Application.Exceptions;
using Aviate.Core.Filters;
using Aviate.Core.Models;
using Aviate.DataAccess.Repositories;

namespace Aviate.Application.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seats;
        //private readonly IValidator<SeatFilter> _filterValidator;

        public SeatService
            (
            ISeatRepository seats
            //IValidator<SeatFilter> filterValidator
            )
        {
            _seats = seats;
            //_filterValidator = filterValidator;

        }

        //Отримати місце по flightId
        public async Task<List<Seat>> GetSeatsByFlightAsync(Guid flightId)
        {
            var seat = await _seats.GetByFlightIdAsync(flightId);
            if (seat == null)
                throw new EntityNotFoundException("Seat", flightId);
            return seat;
        }

        //Отримати місцеи за фільтром
        public async Task<PagedResult<Seat>> GetFilteredAsync(SeatFilter filter)
        {
            // Валідація фільтра
            //await _filterValidator.ValidateAndThrowAsync(filter);

            return await _seats.GetFilteredAsync(filter);
        }
    }
}
