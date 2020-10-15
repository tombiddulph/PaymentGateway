using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application;
using PaymentGateway.Application.Infrastructure;

namespace PaymentGateway.Api.Tests.Integration
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PaymentGatewayDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseInMemoryDatabase(nameof(TestStartup));
            });
            services.AddApplication();
            services.AddMvc()
                .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", o => { });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}