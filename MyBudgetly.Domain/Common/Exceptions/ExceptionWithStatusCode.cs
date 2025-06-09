using Microsoft.Extensions.Logging;

namespace MyBudgetly.Domain.Common.Exceptions;

public class ExceptionWithStatusCode : Exception
{
    public int StatusCode { get; }
    public int? ErrorCode { get; }
    public LogLevel LogLevel { get; }
    public bool ExposeMessage { get; }

    public ExceptionWithStatusCode(int statusCode)
        : this(null, statusCode)
    {
    }

    public ExceptionWithStatusCode(
        string? message,
        int statusCode,
        LogLevel logLevel = LogLevel.Information,
        int? errorCode = null,
        bool exposeMessage = true)
        : base(message)
    {
        StatusCode = statusCode;
        LogLevel = logLevel;
        ErrorCode = errorCode;
        ExposeMessage = exposeMessage;
    }

    public ExceptionWithStatusCode(
        string? message,
        int statusCode,
        Exception? innerException,
        LogLevel logLevel = LogLevel.Information,
        int? errorCode = null,
        bool exposeMessage = true)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        LogLevel = logLevel;
        ErrorCode = errorCode;
        ExposeMessage = exposeMessage;
    }
}