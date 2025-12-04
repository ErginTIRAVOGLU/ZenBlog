using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using ZenBlog.Application.Concrete;

namespace ZenBlog.API.Handlers;
 
 public sealed class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
         Result<string> errorResult;

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = 500;

        var actualException = exception is AggregateException agg && agg.InnerException != null
        ? agg.InnerException
        : exception;

        var exceptionType = actualException.GetType();
        var validationExceptionType = typeof(ValidationException);
 
        if (exceptionType == validationExceptionType)
        {
            httpContext.Response.StatusCode = 422;

            var validationErrors = ((ValidationException)actualException).Errors.Select(e => new Error(e.PropertyName, e.ErrorMessage)).ToList();
            errorResult = Result<string>.Failure(validationErrors);

            await httpContext.Response.WriteAsJsonAsync(errorResult);

            return true;
        }

        
        errorResult = Result<string>.Failure(new Error("Exception", exception.Message));

        await httpContext.Response.WriteAsJsonAsync(errorResult);

        return true;
    }
}