using Aviate.Application.Contracts;
using Aviate.Application.Dto.Booking;
using Aviate.Application.Dto.Payment;
using Aviate.Application.Exceptions;
using Aviate.Core.Contracts;
using Aviate.Core.Enums;
using Aviate.Core.Filters;
using Aviate.Core.Models;
using Aviate.DataAccess.Repositories;
using FluentValidation;
using System.Collections.Generic;

namespace Aviate.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookings;
        private readonly IFlightRepository _flights;
        private readonly ISeatRepository _seats;
        private readonly IUserRepository _users;
        private readonly IPaymentService _paymentService;
        private readonly IValidator<BookingCreateDto> _createValidator;
        //private readonly IValidator<BookingUpdateDto> _updateValidator;
        private readonly IValidator<BookingAdminFilter> _filterValidator;


        public BookingService
            (
            IBookingRepository bookings,
            IFlightRepository flights,
            ISeatRepository seats,
            IUserRepository users,
            IPaymentService paymentService,
            IValidator<BookingCreateDto> createValidator,
            //IValidator<BookingUpdateDto> updateValidator,
            IValidator<BookingAdminFilter> filterValidator
            )
        {
            _bookings = bookings;
            _flights = flights;
            _seats = seats;
            _users = users;
            _paymentService = paymentService;
            _createValidator = createValidator;
            //_updateValidator = updateValidator;
            _filterValidator = filterValidator;

        }

        // Отримати бронювання по ID
        public async Task<Booking> GetByIdAsync(Guid id)
        {
            return await GetBookingByIdAsync(id);
        }

        // Отримати всі бронювання по User ID
        public async Task<List<Booking>> GetByUserIdAsync(Guid userId)
        {
            return await _bookings.GetByUserIdAsync(userId);
        }

        // Отримати бронюванняи за фільтром (для адміна)
        public async Task<PagedResult<Booking>> GetFilteredAsync(BookingAdminFilter filter)
        {
            // Валідація фільтра
            //await _filterValidator.ValidateAndThrowAsync(filter);

            return await _bookings.GetFilteredAsync(filter);
        }

        // Створити бронювання
        public async Task<Booking> CreateAsync(BookingCreateDto request)
        {
            // Валідація запиту
            await _createValidator.ValidateAndThrowAsync(request);

            // Отримати рейс
            var flight = await _flights.GetByIdAsync(request.FlightId) ?? throw new EntityNotFoundException("Flight", request.FlightId);

            // Не можна забронювати місце, якщо рейс вже в польоті, відмінений або завершений
            if (flight.Status is FlightStatus.InFlight or FlightStatus.Cancelled or FlightStatus.Completed)
                throw new ConflictException("You cannot reserve a seat when the flight is already in flight, cancelled or completed.");

            var user = await _users.GetByIdAsync(request.UserId) ?? throw new EntityNotFoundException("User", request.UserId);

            Seat seat;

            if (request.SeatId is null)
            {
                var filter = new SeatFilter
                {
                    FlightId = flight.Id,
                    Class = SeatClass.Economy,
                    IsBooked = false,
                    Page = 1,
                    PageSize = int.MaxValue
                };

                var result = await _seats.GetFilteredAsync(filter);
                var availableEconomySeats = result.Items;

                if (!availableEconomySeats.Any())
                    throw new ConflictException("No available seats in Economy class. Please choose a seat from another class");

                var random = new Random();
                seat = availableEconomySeats[random.Next(availableEconomySeats.Count)];
            }
            else
            {
                seat = await _seats.GetByIdAsync(request.SeatId.Value) ?? throw new EntityNotFoundException("Seat", request.SeatId.Value);
                if (seat.IsBooked)
                    throw new ConflictException("Seat is already booked");
            }

            decimal totalPrice = flight.BasePrice;

            //Налог аєропорту
            decimal airportFee = 50;
            decimal businessCoefficient = 2.5m;
            decimal firstCoefficient = 3m;

            if (seat.Class == SeatClass.Business)
            {
                totalPrice *= businessCoefficient;
            }
            else if (seat.Class == SeatClass.First)
            {
                totalPrice *= firstCoefficient;
            }

            // Додаємо налог до собівартості квитка
            totalPrice += airportFee;

            // Створення бронювання
            var booking = Booking.Create(
                user,
                flight,
                seat,
                totalPrice
            );

            await _bookings.AddAsync(booking);
            return booking;
        }

        // Скасувати бронювання
        public async Task CancelBookingAsync(Guid userId, Guid bookingId)
        {
            var booking = await GetBookingByIdAsync(bookingId);
            if (booking.Status == BookingStatus.Cancelled) return;
            if (booking.UserId != userId) throw new UnauthorizedAccessException("User cannot cancel someone else's booking.");
            booking.Cancel();
            await _bookings.UpdateAsync(booking);
        }

        // Оплатити бронювання
        public async Task<PaymentResult> PayBookingAsync(Guid userId, Guid bookingId, PaymentMethod paymentMethod)
        {
            var booking = await GetBookingByIdAsync(bookingId);
            if (booking.Status == BookingStatus.Cancelled) throw new ConflictException("Booking is cancelled and cannot be paid");
            if (booking.Status == BookingStatus.Paid) throw new ConflictException("Booking is already paid");
            if (booking.UserId != userId) throw new UnauthorizedAccessException("User cannot pay for someone else's booking.");

            var paymentResult = await _paymentService.ProcessPaymentAsync(booking, booking.TotalPrice, paymentMethod);
            if (paymentResult.IsSuccessful)
            {
                booking.MarkAsPaid();
                await _bookings.UpdateAsync(booking);

            }
            return paymentResult;
        }

        // Видалити бронювання
        public async Task DeleteAsync(Guid id)
        {
            var booking = await GetBookingByIdAsync(id);
            if (booking.Status == BookingStatus.Paid) throw new ConflictException("The reservation cannot be deleted, it has already been paid for.");
            await _bookings.DeleteAsync(booking);
        }

        private async Task<Booking> GetBookingByIdAsync(Guid id)
        {
            var booking = await _bookings.GetByIdAsync(id);
            if (booking is null)
                throw new EntityNotFoundException("Booking", id);
            return booking;
        }
    }
}
