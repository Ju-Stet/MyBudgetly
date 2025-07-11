using System.Net;
using System.Text.Json;
using MyBudgetly.Application.Common.Models;
using MyBudgetly.Domain.Common.Exceptions;

namespace MyBudgetly.API.Middleware;

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
        context.Response.ContentType = "application/json";

        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "An unexpected error occurred.";

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

        context.Response.StatusCode = statusCode;

        var errorResponse = ApiResponse<string>.Failure(message, statusCode);
        var json = JsonSerializer.Serialize(errorResponse);

        await context.Response.WriteAsync(json);
    }
}