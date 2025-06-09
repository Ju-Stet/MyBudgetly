using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyBudgetly.Domain.Common.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        int statusCode = (int)HttpStatusCode.InternalServerError;
        string message = "An unexpected error occurred.";

        if (exception is ExceptionWithStatusCode exWithStatus)
        {
            statusCode = exWithStatus.StatusCode;
            message = exWithStatus.ExposeMessage ? exWithStatus.Message : message;
            logger.Log(exWithStatus.LogLevel, exception, exWithStatus.Message);
        }
        else
        {
            logger.LogError(exception, exception.Message);
        }

        response.StatusCode = statusCode;

        var result = JsonSerializer.Serialize(new
        {
            error = message,
            statusCode = response.StatusCode
        });

        await response.WriteAsync(result);
    }
}