using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.User.Booking;
using Aviate.API.Dto.User.Flight;
using Aviate.Application.Services;
using Aviate.Core.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers
{
    // ================= FLIGHT =================
    [Route("api/flights")]
    [ApiController]
    [AllowAnonymous]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;

        public FlightsController(IFlightService flightsService, IMapper mapper)
        {
            _flightService = flightsService;
            _mapper = mapper;
        }

        /// <summary>Отримати рейс по ID</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var flight = await _flightService.GetByIdAsync(id);
            var response = _mapper.Map<GetFlightBookingResponse>(flight);
            return Ok(response);
        }

        /// <summary>Отримати рейси за фільтром</summary>
        [HttpGet]
        public async Task<IActionResult> GetFiltered([FromQuery] FlightFilter filter)
        {
            var flights = await _flightService.GetFilteredAsync(filter);
            var response = _mapper.Map<PagedResultResponse<GetFlightResponse>>(flights);
            return Ok(response);
        }
    }
}
