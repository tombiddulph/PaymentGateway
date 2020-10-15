using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace PaymentGateway.Api.Tests.Integration
{
    public class CustomWebAppFactory : WebApplicationFactory<TestStartup> 
    {

        protected override IHost CreateHost(IHostBuilder builder)
        {
            // https://github.com/dotnet/aspnetcore/issues/17707
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            return base.CreateHost(builder);
        }
        
        protected override IHostBuilder CreateHostBuilder()
        {
            var host = Host.CreateDefaultBuilder().ConfigureWebHostDefaults(h => h.UseStartup<TestStartup>());
            return host;
        }
    }
}