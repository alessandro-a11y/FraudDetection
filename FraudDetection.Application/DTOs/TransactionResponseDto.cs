using FraudDetection.Domain.Enums;

namespace FraudDetection.Application.DTOs;

public class TransactionResponseDto
{
    public Guid Id { get; set; }
    public FraudRiskLevel RiskLevel { get; set; }
    public int RiskScore { get; set; }
}