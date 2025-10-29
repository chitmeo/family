using System.ComponentModel.DataAnnotations;

using Dev.Exceptions;

using Microsoft.AspNetCore.Diagnostics;

namespace Dev.WebHost.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";
        var (title, detail, statusCode, errors) = exception switch
        {
            // 400 - Validation failed
            ValidationException ex => (
                Title: "Validation failed",
                Detail: ex.Message,
                StatusCode: StatusCodes.Status400BadRequest,
                Errors: null as object
            ),
            //404 - Not found
            KeyNotFoundException ex => (
                Title: "Resource not found",
                Detail: ex.Message,
                StatusCode: StatusCodes.Status404NotFound,
                Errors: null as object
            ),
            //409 - Conflict
            InvalidOperationException ex => (
                Title: "Conflict",
                Detail: ex.Message,
                StatusCode: StatusCodes.Status409Conflict,
                Errors: null as object
            ),
            //422 - Unprocessable Entity
            ValidationAppException ex => (
                Title: "Unprocessable entity",
                Detail: ex.Message,
                StatusCode: StatusCodes.Status422UnprocessableEntity,
                Errors: ex.Errors
            ),
            //500 - Server Error
            _ => (
                Title: "Internal Server Error",
                Detail: exception.Message,
                StatusCode: StatusCodes.Status500InternalServerError,
                Errors: null as object
            )
        };
        httpContext.Response.StatusCode = statusCode;

        if (errors != null)
        {
            await httpContext.Response.WriteAsJsonAsync(new
            {
                title,
                detail,
                errors,
                status = statusCode
            }, cancellationToken);

            return true;
        }

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Title = title,
                Detail = detail,
                Status = statusCode
            }
        });
    }
}
