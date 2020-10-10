using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Api
{
    
    class t : ExceptionFilterAttribute{}
    
    
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(
            IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        
        

        public override Task OnExceptionAsync(ExceptionContext context)
        {


            context.Result = new UnprocessableEntityResult();
            
            
            using var scope = _logger.BeginScope("Exception filter");
            _logger.LogError(context.Exception, context.Exception.Message);
            
            context.ExceptionHandled = true;
            if (_hostingEnvironment.IsDevelopment())
            {
                context.Result = new UnauthorizedObjectResult(context.Exception);
                return Task.CompletedTask;
            }

            context.Result = new UnprocessableEntityResult();
            
            return Task.CompletedTask;
        }
    }
}