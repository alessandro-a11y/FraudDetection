using FraudDetection.Domain.Entities;
using FraudDetection.Domain.Interfaces;

namespace FraudDetection.Domain.Rules;

public class LocationMismatchRule : IFraudRule
{
    public string Name => "LocationMismatch";

    public int Evaluate(Transaction transaction)
    {
        return transaction.Location != "BR" ? 30 : 0;
    }
}