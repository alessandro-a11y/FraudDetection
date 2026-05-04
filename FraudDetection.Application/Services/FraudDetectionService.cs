using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FraudDetection.Application.Services;

public class FraudDetectionService
{
    private readonly ITransactionRepository _repository;
    private readonly ILogger<FraudDetectionService> _logger;

    public FraudDetectionService(
        ITransactionRepository repository,
        ILogger<FraudDetectionService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task AnalyzeAsync(Transaction transaction)
    {
        var recent = await _repository.GetRecentByUserAsync(
            transaction.UserId,
            TimeSpan.FromMinutes(1));

        if (recent.Count >= 3)
        {
            _logger.LogWarning(
                "Velocidade suspeita detectada: usuário {UserId} fez {Count} transações no último minuto.",
                transaction.UserId, recent.Count);

            typeof(Transaction)
                .GetProperty("RiskScore")!
                .SetValue(transaction, transaction.RiskScore + 20);
        }

        _logger.LogInformation(
            "Transação {TransactionId} analisada: RiskScore={RiskScore}, RiskLevel={RiskLevel}",
            transaction.Id, transaction.RiskScore, transaction.RiskLevel);
    }
}