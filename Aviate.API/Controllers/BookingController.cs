using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.User.Booking;
using Aviate.Application.Contracts;
using Aviate.Application.Dto.Booking;
using Aviate.Application.Exceptions;
using Aviate.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers.Admin
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
            var bookings = await _bookingService.CreateAsync(newRequest);
            return Ok(bookings);
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
        public async Task<IActionResult> GetMyBookings()
        {
            var UserId = GetUserId();
            var bookings = await _bookingService.GetByUserIdAsync(UserId);

            var response = _mapper.Map<List<GetBookingResponse>>(bookings);
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
