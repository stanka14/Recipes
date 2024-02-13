using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Service.ErrorHandling
{
    public class ErrorHandlingFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(context.Exception.Message)
            {
                StatusCode = 500,
            };
            context.ExceptionHandled = true;
            context.ExceptionHandled = true;
        }
    }
}
