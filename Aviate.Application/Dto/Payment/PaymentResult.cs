namespace Aviate.Application.Dto.Payment
{
    public record PaymentResult(bool IsSuccessful, string? TransactionId, string? ErrorMessage);
}