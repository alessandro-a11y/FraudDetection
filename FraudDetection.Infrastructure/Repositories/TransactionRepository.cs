using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Interfaces;
using FraudDetection.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FraudDetection.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Transaction>> GetRecentByUserAsync(string userId, TimeSpan interval)
    {
        var cutoff = DateTime.UtcNow - interval;

        return await _context.Transactions
            .Where(t => t.UserId == userId && t.CreatedAt >= cutoff)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}