using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application.Services;

namespace PaymentGateway.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITransactionService, TransactionService>();
            return serviceCollection;
        }
    }
}