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
            serviceCollection.AddTransient<ITransactionService, TransactionService>();
            serviceCollection.AddSingleton<IRepository<Card>, Repository<Card>>();
            serviceCollection.AddSingleton<IRepository<Merchant>, Repository<Merchant>>();
            return serviceCollection;
        }
    }
}