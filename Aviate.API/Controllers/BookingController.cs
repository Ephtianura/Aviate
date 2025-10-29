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

namespace Aviate.API.Controllers
{
    // ================= BOOKINGS =================
    [Route("api/bookings")]
    [ApiController]
    [Authorize(Policy = "UserPolicy")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingsController(IBookingService bookingsService, IMapper mapper)
        {
            _bookingService = bookingsService;
            _mapper = mapper;
        }
        
        /// <summary>Створити бронювання</summary>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BookingCreateRequest request)
        {
            var UserId = GetUserId();
            var newRequest = new BookingCreateDto(UserId, request.FlightId, request.SeatId);
            await _bookingService.CreateAsync(newRequest);
            return Ok(new ApiResponse("Booking successfully created"));

        }

        /// <summary>Оплатити бронювання</summary>
        [HttpPost("{bookingId:guid}/pay")]
        public async Task<IActionResult> PayBooking(Guid bookingId, [FromQuery] PaymentMethod paymentMethod)
        {
            var UserId = GetUserId();
            var paymentResult = await _bookingService.PayBookingAsync(UserId, bookingId, paymentMethod);

            if (paymentResult.IsSuccessful)
            {
                return Ok(paymentResult);
            }
            else
            {
                return StatusCode(StatusCodes.Status424FailedDependency, paymentResult);
            }
        }
        /// <summary>Скасувати бронювання</summary>
        [HttpPost("{bookingId:guid}/cancel")]
        public async Task<IActionResult> CancelBooking(Guid bookingId)
        {
            var UserId = GetUserId();
            await _bookingService.CancelBookingAsync(UserId, bookingId);
            return Ok();
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookings([FromQuery] BookingUserFilter filter)
        {
            var UserId = GetUserId();
            var _filter = _mapper.Map<BookingAdminFilter>(filter);
            _filter.UserId = UserId;

            var bookings = await _bookingService.GetFilteredAsync(_filter);
            var response = _mapper.Map<PagedResultResponse<GetBookingResponse>>(bookings);
            return Ok(response);
        }

        private Guid GetUserId()
        {
            // Виймаємо userId з токена
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new MissingUserIdClaimException();
            if (!Guid.TryParse(userIdClaim, out Guid userId))
                throw new InvalidUserIdFormatException();

            return userId;
        }
    }
}
