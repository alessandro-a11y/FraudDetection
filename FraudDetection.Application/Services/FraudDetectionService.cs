using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Interfaces;
using FraudDetection.Domain.Rules;
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

        // Monta as regras dinamicamente
        var rules = new List<IFraudRule>
        {
            new HighAmountRule(),
            new LocationMismatchRule(),
            new VelocityRule(recent)
        };

        var engine = new FraudRuleEngine(rules);
        var score = engine.Evaluate(transaction);

        transaction.ApplyRiskScore(score);

        _logger.LogInformation(
            "Transação {TransactionId} analisada: RiskScore={RiskScore}, RiskLevel={RiskLevel}",
            transaction.Id, transaction.RiskScore, transaction.RiskLevel);

        if (transaction.RiskLevel == Domain.Enums.FraudRiskLevel.HIGH)
        {
            _logger.LogWarning(
                "FRAUDE DETECTADA: Usuário {UserId} | Score={RiskScore} | Location={Location} | Amount={Amount}",
                transaction.UserId, transaction.RiskScore, transaction.Location, transaction.Amount);
        }
    }
}