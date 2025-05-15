using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.User.Booking;
using Aviate.Application.Contracts;
using Aviate.Application.Dto.Booking;
using Aviate.Application.Exceptions;
using Aviate.Core.Enums;
using Aviate.Core.Filters;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers.Admin
{
    // ================= ADMIN-BOOKINGS =================
    [Route("api/admin/bookings")]
    [ApiController]

    [Authorize(Policy = "AdminPolicy")]
    public class AdminBookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public AdminBookingsController(IBookingService bookingsService, IMapper mapper)
        {
            _bookingService = bookingsService;
            _mapper = mapper;
        }

        /// <summary>Отримати бронювання по ID</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            var response = _mapper.Map<GetBookingResponse>(booking);
            return Ok(response);
        }

        /// <summary>Отримати бронювання за фільтром</summary>
        [HttpGet]
        public async Task<IActionResult> GetFiltered([FromQuery] BookingFilter filter)
        {
            var bookings = await _bookingService.GetFilteredAsync(filter);
            var response = _mapper.Map<PagedResultResponse<GetBookingResponse>>(bookings);
            return Ok(response);
        }

        /// <summary>Видалити бронювання</summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookingService.DeleteAsync(id);
            return NoContent();
        }
    }
}
