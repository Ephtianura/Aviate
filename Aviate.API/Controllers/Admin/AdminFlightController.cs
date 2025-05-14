using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.Admin;
using Aviate.API.Dto.User;
using Aviate.Application.Contracts;
using Aviate.Application.Dto.Flight;
using Aviate.Application.Services;
using Aviate.Core.Filters;
using Aviate.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers.Admin
{
    // ================= ADMIN-AIRPLANES =================
    [Route("api/admin/flights")]
    [ApiController]

    [Authorize(Policy = "AdminPolicy")]
    public class AdminFlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;

        public AdminFlightsController(IFlightService flightsService, IMapper mapper)
        {
            _flightService = flightsService;
            _mapper = mapper;
        }

        /// <summary>Отримати рейс по ID</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var flight = await _flightService.GetByIdAsync(id);
            var response = _mapper.Map<GetFlightResponse>(flight);
            return Ok(response);
        }

        /// <summary>Отримати рейси за фільтром</summary>
        [HttpGet]
        public async Task<IActionResult> GetFiltered([FromQuery] FlightFilter filter)
        {
            var flights = await _flightService.GetFilteredAsync(filter);
            var response = _mapper.Map<PagedResultResponse<GetFlightsResponse>>(flights);
            return Ok(flights);
        }

        /// <summary>Створити рейс</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingCreateDto request)
        {
            var flights = await _flightService.CreateAsync(request);
            return Ok(new ApiResponse("Flight successfully created"));
        }

        /// <summary>Оновити рейс</summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] FlightUpdateDto dto)
        {
            await _flightService.UpdateAsync(id, dto);
            return NoContent();
        }

        /// <summary>Видалити рейс</summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _flightService.DeleteAsync(id);
            return NoContent();
        }
    }
}
