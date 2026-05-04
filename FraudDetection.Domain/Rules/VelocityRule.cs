using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Interfaces;

namespace FraudDetection.Domain.Rules;

public class VelocityRule : IFraudRule
{
    private readonly List<Transaction> _recentTransactions;

    public string Name => "Velocity";

    public VelocityRule(List<Transaction> recentTransactions)
    {
        _recentTransactions = recentTransactions;
    }

    public int Evaluate(Transaction transaction)
    {
        return _recentTransactions.Count >= 3 ? 20 : 0;
    }
}