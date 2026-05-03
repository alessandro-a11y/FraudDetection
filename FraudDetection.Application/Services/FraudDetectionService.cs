using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Interfaces;

namespace FraudDetection.Application.Services;

public class FraudDetectionService
{
    private readonly ITransactionRepository _repository;

    public FraudDetectionService(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task AnalyzeAsync(Transaction transaction)
    {
        var recent = await _repository.GetRecentByUserAsync(
            transaction.UserId,
            TimeSpan.FromMinutes(1));

        if (recent.Count >= 3)
        {
            // aumenta risco se muitas transações rápidas
            typeof(Transaction)
                .GetProperty("RiskScore")!
                .SetValue(transaction, transaction.RiskScore + 20);
        }
    }
}