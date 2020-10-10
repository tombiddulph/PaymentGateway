using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Data.Models;

namespace PaymentGateway.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
           
            return services;
        }
    }
}