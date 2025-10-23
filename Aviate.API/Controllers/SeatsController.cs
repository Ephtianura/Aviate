using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.Admin;
using Aviate.API.Dto.User;
using Aviate.API.Dto.User.Booking;
using Aviate.Application.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers
{
    // ================= SEATS =================
    [Route("api/seats")]
    [ApiController]

    [Authorize(Policy = "UserPolicy")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;
        private readonly IMapper _mapper;

        public SeatsController(ISeatService seatsService, IMapper mapper)
        {
            _seatService = seatsService;
            _mapper = mapper;
        }

        /// <summary>Отримати місця по ID рейсу</summary>
        //[HttpGet("{flightId}")]
        //public async Task<IActionResult> GetById(Guid flightId)
        //{
        //    var seats = await _seatService.GetSeatsByFlightAsync(flightId);
        //    var response = _mapper.Map<PagedResultResponse<GetSeatResponse>>(seats);
        //    return Ok(response);
        //}

        /// <summary>Отримати місця за фільтром</summary>
        [HttpGet]
        public async Task<IActionResult> GetFiltered([FromQuery] SeatFilter filter)
        {
            var seats = await _seatService.GetFilteredAsync(filter);
            var response = _mapper.Map<PagedResultResponse<GetSeatAdminResponse>>(seats);
            return Ok(response);
        }

    }
}
