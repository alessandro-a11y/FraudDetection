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

        EvaluateRisk();
    }

    private void EvaluateRisk()
    {
        int score = 0;

        // Regra 1: valor alto
        if (Amount > 5000)
            score += 40;

        // Regra 2: valor muito alto
        if (Amount > 10000)
            score += 30;

        // Regra 3: localização suspeita
        if (Location != "BR")
            score += 30;

        RiskScore = score;

        RiskLevel = score switch
        {
            <= 30 => FraudRiskLevel.LOW,
            <= 70 => FraudRiskLevel.MEDIUM,
            _ => FraudRiskLevel.HIGH
        };
    }
}