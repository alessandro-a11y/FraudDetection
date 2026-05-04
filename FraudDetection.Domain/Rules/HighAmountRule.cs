using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Interfaces;

namespace FraudDetection.Domain.Rules;

public class HighAmountRule : IFraudRule
{
    public string Name => "HighAmount";

    public int Evaluate(Transaction transaction)
    {
        int score = 0;
        if (transaction.Amount > 5000) score += 40;
        if (transaction.Amount > 10000) score += 30;
        return score;
    }
}