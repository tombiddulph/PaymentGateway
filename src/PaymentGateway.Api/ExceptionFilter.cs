using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Api
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(
            IWebHostEnvironment hostingEnvironment, ILogger<ExceptionFilter> logger)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public override Task OnExceptionAsync(ExceptionContext context)
        {
            context.Result = new UnprocessableEntityResult();

            context.ExceptionHandled = true;
            if (_hostingEnvironment.IsDevelopment())
            {
                _logger.LogError("An unknown error occured", context.Exception);
                context.Result = new UnprocessableEntityObjectResult(context.Exception.Message);
                return Task.CompletedTask;
            }

            _logger.LogError("An unknown error occured");
            context.Result = new UnprocessableEntityResult();

            return Task.CompletedTask;
        }
    }
}