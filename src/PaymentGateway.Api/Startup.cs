using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PaymentGateway.Api.Auth;
using PaymentGateway.Application;
using PaymentGateway.Application.Infrastructure;

namespace PaymentGateway.Api
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PaymentGatewayDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlite(_configuration.GetSection("ConnectionString").Value);
            });

            services.AddControllers(cfg => cfg.Filters.Add<ExceptionFilter>())
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddApplication();
            services.AddMvc().AddJsonOptions(
                options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            );

            services.AddLogging();
            services.PostConfigure<ApiBehaviorOptions>(ConfigureApiBehaviorOptions);

            services.AddSwaggerGen(config =>
            {
                var openApiInfo = new OpenApiInfo();
                _configuration.GetSection(nameof(OpenApiInfo)).Bind(openApiInfo);
                config.SwaggerDoc("v1", openApiInfo);
                config.DescribeAllParametersInCamelCase();
                //
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);

                config.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic auth (for demo purposes)"
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[] { }
                    }
                });
            });
            services.AddHealthChecks()
                .AddCheck("HeartBeat", () => HealthCheckResult.Healthy(), new[] {"HeartBeat"})
                .AddCheck("AlwaysUnhealthy", () => HealthCheckResult.Unhealthy());

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.AddScoped<IUserService, UserService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseSwagger().UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway v1");
                config.RoutePrefix = string.Empty;
            });


            app.UseRouting();
            app.UseAuthentication().UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/heartbeat", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("HeartBeat")
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