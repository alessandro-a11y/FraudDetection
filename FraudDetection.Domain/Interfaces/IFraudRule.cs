using FraudDetection.Domain.Entities;

namespace FraudDetection.Domain.Interfaces;

public interface IFraudRule
{
    string Name { get; }
    int Evaluate(Transaction transaction);
}