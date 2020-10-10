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
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(
            IWebHostEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider,
            ILogger<ExceptionFilter> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
            _logger = logger;
        }
        

        public void OnException(ExceptionContext context)
        {
            using var scope = _logger.BeginScope("Exception filter");
            _logger.LogError(context.Exception, context.Exception.Message);
            
            context.ExceptionHandled = true;
            if (_hostingEnvironment.IsDevelopment())
            {
                context.Result = new UnauthorizedObjectResult(context.Exception);
                return;
            }

            context.Result = new UnprocessableEntityResult();
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {


            ErrorDetails errorDetails = new ErrorDetails();
            if (context.Exception is ApiException apiException)
            {
                errorDetails.Message = apiException.Message;
                er
            }

            if (_hostingEnvironment.IsDevelopment())
            {
                
            }
            
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