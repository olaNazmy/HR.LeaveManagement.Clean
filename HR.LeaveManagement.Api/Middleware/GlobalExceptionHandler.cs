using HR.LeaveManagement.Api.Middleware.Models;
using HR.LeaveManagement.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HR.LeaveManagement.Api.Middleware
{
    public class GlobalExceptionHandler:IExceptionHandler
    {
        private readonly IHostEnvironment _environment;

        public GlobalExceptionHandler(IHostEnvironment environment)
        {
            this._environment = environment;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // here is the handling 
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            ProblemDetails problem;

            //
            switch (exception)
            {
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    problem = new CustomProblemValidationDetails
                    {
                        Title = _environment.IsDevelopment() ? badRequestException.Message : "An error occurred processing your request.",
                        Status = (int)statusCode,
                        Detail = badRequestException.InnerException?.Message,
                        Type = nameof(BadRequestException),
                        Errors = badRequestException.ValidationErrors
                    };
                    break;
                case NotFoundException NotFound:
                    statusCode = HttpStatusCode.NotFound;
                    problem = new ProblemDetails
                    {
                        Title = _environment.IsDevelopment() ? NotFound.Message : "An error occurred processing your request.",
                        Status = (int)statusCode,
                        Type = nameof(NotFoundException),
                        Detail = NotFound.InnerException?.Message,
                    };
                    break;
                default:
                    problem = new ProblemDetails
                    {
                        Title = _environment.IsDevelopment() ? exception.Message : "An error occurred processing your request.",
                        Status = (int)statusCode,
                        Type = nameof(HttpStatusCode.InternalServerError),
                        //use env for wrapping the stacktrace for security
                        //Detail = ex.StackTrace
                        Detail = _environment.IsDevelopment() ? exception.StackTrace : "An unexpected error occurred."
                    };
                    break;
            }

            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync<object>(problem,cancellationToken);

            return true;

        }
    }
}
