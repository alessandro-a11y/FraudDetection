using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Interfaces;

namespace FraudDetection.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private static readonly List<Transaction> _storage = new();

    public Task AddAsync(Transaction transaction)
    {
        _storage.Add(transaction);
        return Task.CompletedTask;
    }

    public Task<List<Transaction>> GetRecentByUserAsync(string userId, TimeSpan interval)
    {
        var cutoff = DateTime.UtcNow - interval;

        var result = _storage
            .Where(t => t.UserId == userId && t.CreatedAt >= cutoff)
            .ToList();

        return Task.FromResult(result);
    }
}