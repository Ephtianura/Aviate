using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.User;
using Aviate.Application.Dto.Flight;
using Aviate.Application.Services;
using Aviate.Core.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers.Admin
{
    // ================= ADMIN-FLIGHT =================
    [Route("api/admin/flights")]
    [ApiController]

    [Authorize(Policy = "EmployeePolicy")]
    public class AdminFlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;

        public AdminFlightsController(IFlightService flightsService, IMapper mapper)
        {
            _flightService = flightsService;
            _mapper = mapper;
        }

        /// <summary>Створити рейс</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FlightCreateDto request)
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
