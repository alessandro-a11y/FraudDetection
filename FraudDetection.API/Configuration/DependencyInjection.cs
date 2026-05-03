using FraudDetection.Application.Services;
using FraudDetection.Application.UseCases;
using FraudDetection.Domain.Interfaces;
using FraudDetection.Infrastructure.Repositories;

namespace FraudDetection.API.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ITransactionRepository, TransactionRepository>();

        services.AddScoped<CreateTransactionUseCase>();
        services.AddScoped<FraudDetectionService>();

        return services;
    }
}