using FraudDetection.Domain.Entities;

namespace FraudDetection.Domain.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<List<Transaction>> GetRecentByUserAsync(string userId, TimeSpan interval);
}