using FraudDetection.Domain.Enums;

namespace FraudDetection.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; }
    public decimal Amount { get; private set; }
    public string Location { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public FraudRiskLevel RiskLevel { get; private set; }
    public int RiskScore { get; private set; }

    private Transaction() { } // EF

    public Transaction(string userId, decimal amount, string location)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Amount = amount;
        Location = location;
        CreatedAt = DateTime.UtcNow;
    }

    public void ApplyRiskScore(int score)
    {
        RiskScore = score;
        RiskLevel = score switch
        {
            <= 30 => FraudRiskLevel.LOW,
            <= 70 => FraudRiskLevel.MEDIUM,
            _ => FraudRiskLevel.HIGH
        };
    }
}