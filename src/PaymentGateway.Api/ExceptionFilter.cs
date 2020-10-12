using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Api
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ExceptionFilter(
            IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        public override Task OnExceptionAsync(ExceptionContext context)
        {
            context.Result = new UnprocessableEntityResult();

            //todo log and properly handle exception

            context.ExceptionHandled = true;
            if (_hostingEnvironment.IsDevelopment())
            {
                context.Result = new UnprocessableEntityObjectResult(context.Exception.Message);
                return Task.CompletedTask;
            }

            context.Result = new UnprocessableEntityResult();

            return Task.CompletedTask;
        }
    }
}