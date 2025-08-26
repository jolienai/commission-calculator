using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace FCamara.Commission.Api.Middlewares;

public class ExceptionLoggingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionLoggingMiddleware> logger,
    IHostEnvironment env)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unhandled exception occurred while processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "An unexpected error occurred",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = env.IsDevelopment() ? ex.Message : "An internal server error occurred.",
                Instance = context.Request.Path
            };

            context.Response.StatusCode = problemDetails.Status.Value;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}