namespace FraudDetection.Application.DTOs;

public class CreateTransactionDto
{
    public string UserId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Location { get; set; } = string.Empty;
}