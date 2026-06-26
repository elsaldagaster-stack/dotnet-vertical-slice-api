using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Domain.Exceptions;

namespace TaskFlow.Api.Infrastructure.Common.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, detail) = exception switch
        {
            ValidationException ve => (
                StatusCodes.Status422UnprocessableEntity,
                "Validation failed",
                string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))),
            DomainException de => (
                StatusCodes.Status422UnprocessableEntity,
                "Business rule violation",
                de.Message),
            KeyNotFoundException ke => (
                StatusCodes.Status404NotFound,
                "Resource not found",
                ke.Message),
            _ => (
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred",
                "Please try again later.")
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = context.Request.Path
        });
    }
}
