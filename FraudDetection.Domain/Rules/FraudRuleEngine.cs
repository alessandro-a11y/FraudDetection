using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Interfaces;

namespace FraudDetection.Domain.Rules;

public class FraudRuleEngine
{
    private readonly IEnumerable<IFraudRule> _rules;

    public FraudRuleEngine(IEnumerable<IFraudRule> rules)
    {
        _rules = rules;
    }

    public int Evaluate(Transaction transaction)
    {
        int totalScore = 0;

        foreach (var rule in _rules)
        {
            var score = rule.Evaluate(transaction);
            totalScore += score;
        }

        return totalScore;
    }
}