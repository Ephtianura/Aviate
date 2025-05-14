using Aviate.Application.Contracts;
using Aviate.Application.Dto.Payment;
using Aviate.Application.Exceptions;
using Aviate.Core.Contracts;
using Aviate.Core.Enums;
using Aviate.Core.Filters;
using Aviate.Core.Models;
using Aviate.DataAccess.Repositories;
using Aviate.Infrastructure.Payment;
using FluentValidation;
using System.Collections.Generic;

namespace Aviate.Application.Services
    {
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _payments;
        private readonly IFlightRepository _flights;
        private readonly ISeatRepository _seats;
        private readonly IUserRepository _users;
        //private readonly IValidator<PaymentCreateDto> _createValidator;
        //private readonly IValidator<PaymentUpdateDto> _updateValidator;
        private readonly IValidator<PaymentFilter> _filterValidator;
        private readonly IPaymentGatewayMock _paymentGatewayMock;

        public PaymentService
            (
            IPaymentRepository payments,
            IFlightRepository flights,
            ISeatRepository seats,
            IUserRepository users,
            IPaymentGatewayMock paymentGatewayMock,
            //IValidator<PaymentCreateDto> createValidator,
            //IValidator<PaymentUpdateDto> updateValidator,
            IValidator<PaymentFilter> filterValidator
            )
        {
            _payments = payments;
            _flights = flights;
            _seats = seats;
            _users = users;
            _paymentGatewayMock = paymentGatewayMock;
            //_createValidator = createValidator;
            //_updateValidator = updateValidator;
            _filterValidator = filterValidator;

        }

        // Отримати оплату по ID
        public async Task<Payment> GetByIdAsync(Guid id)
        {
            return await GetPaymentByIdAsync(id);
        }

        // Отримати всі оплати по User ID
        public async Task<List<Payment>> GetByBookingIdAsync(Guid bookingId)
        {
            return await _payments.GetByBookingIdAsync(bookingId);
        }

        // Отримати оплати за фільтром (для адміна)
        public async Task<PagedResult<Payment>> GetFilteredAsync(PaymentFilter filter)
        {
            // Валідація фільтра
            //await _filterValidator.ValidateAndThrowAsync(filter);
            return await _payments.GetFilteredAsync(filter);
        }

        // Створити оплату
        public async Task<PaymentResult> ProcessPaymentAsync(Booking booking, decimal amount, PaymentMethod paymentMethod)
        {
            // Валідація запиту
            //await _createValidator.ValidateAndThrowAsync(request);

            // Створення оплати
            var payment = Payment.Create(
                booking,
                paymentMethod,
                amount
            );

            var paymentResult = await _paymentGatewayMock.ProcessPaymentAsync(booking.Id, amount, paymentMethod);
            if (paymentResult.IsSuccessful)
            {
                payment.MarkSuccess();
            }
            else
            {
                payment.MarkFailed();
            }

            await _payments.AddAsync(payment);

            return paymentResult;
        }

        private async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            var booking = await _payments.GetByIdAsync(id);
            if (booking is null)
                throw new EntityNotFoundException("Payment", id);
            return booking;
        }
    }
}
