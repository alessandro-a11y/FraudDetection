using FraudDetection.Application.DTOs;
using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Interfaces;

namespace FraudDetection.Application.UseCases;

public class CreateTransactionUseCase
{
    private readonly ITransactionRepository _repository;

    public CreateTransactionUseCase(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<TransactionResponseDto> ExecuteAsync(CreateTransactionDto dto)
    {
        var transaction = new Transaction(dto.UserId, dto.Amount, dto.Location);

        await _repository.AddAsync(transaction);

        return new TransactionResponseDto
        {
            Id = transaction.Id,
            RiskLevel = transaction.RiskLevel,
            RiskScore = transaction.RiskScore
        };
    }
}