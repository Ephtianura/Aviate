namespace Aviate.Application.Services
{
    public record PaymentResult(bool IsSuccessful, string? TransactionId, string? ErrorMessage);
}