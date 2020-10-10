using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PaymentGateway.Application;
using PaymentGateway.Data;

namespace PaymentGateway.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GatewayDbContext>(options =>
            {
                options.UseSqlite("Data Source=PaymentGateway.db;");
            });

            services.AddApplication();
            services.AddMvc().AddJsonOptions(
                options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddLogging();
            services.PostConfigure<ApiBehaviorOptions>(ConfigureApiBehaviorOptions);
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Payment Gateway",
                    Description = "Checkout.com Payment Gateway challenge",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Tom Biddulph",
                        Email = "tom AT thomasbiddulph DOT com",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                config.DescribeAllParametersInCamelCase();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });
            services.AddHealthChecks()
                .AddCheck("HeartBeat", () => HealthCheckResult.Healthy(), new[] {"HeartBeat"})
                .AddCheck("AlwaysUnhealthy", () => HealthCheckResult.Unhealthy());
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger().UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway v1");
                config.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/heartbeat", new HealthCheckOptions
                {
                    Predicate = (check) => check.Tags.Contains("HeartBeat")
                });
            });
        }


        private static void ConfigureApiBehaviorOptions(ApiBehaviorOptions opts)
        {
            var factory = opts.InvalidModelStateResponseFactory;

            opts.InvalidModelStateResponseFactory = ctx =>
            {
                var loggerFactory = ctx.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(ctx.ActionDescriptor.DisplayName);

                logger.LogInformation("bad request");

                return factory(ctx);
            };
        }
    }
}