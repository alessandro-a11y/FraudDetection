using FraudDetection.Application.DTOs;
using FraudDetection.Application.Services;
using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Interfaces;

namespace FraudDetection.Application.UseCases;

public class CreateTransactionUseCase
{
    private readonly ITransactionRepository _repository;
    private readonly FraudDetectionService _fraudService;

    public CreateTransactionUseCase(
        ITransactionRepository repository,
        FraudDetectionService fraudService)
    {
        _repository = repository;
        _fraudService = fraudService;
    }

    public async Task<TransactionResponseDto> ExecuteAsync(CreateTransactionDto dto)
    {
        var transaction = new Transaction(dto.UserId, dto.Amount, dto.Location);

        // Analisa antes de salvar
        await _fraudService.AnalyzeAsync(transaction);

        await _repository.AddAsync(transaction);

        return new TransactionResponseDto
        {
            Id = transaction.Id,
            RiskLevel = transaction.RiskLevel,
            RiskScore = transaction.RiskScore
        };
    }
}