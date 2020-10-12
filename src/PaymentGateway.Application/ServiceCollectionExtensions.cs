using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application.Infrastructure;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Services;

namespace PaymentGateway.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRepository<Card>, Repository<Card>>();
            serviceCollection.AddScoped<IRepository<Merchant>, Repository<Merchant>>();
            serviceCollection.AddScoped<IRepository<Transaction>, Repository<Transaction>>();
            serviceCollection.AddScoped<ITransactionService, TransactionService>();
            return serviceCollection;
        }
    }
}