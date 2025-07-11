using Microsoft.Extensions.Logging;

namespace MyBudgetly.Domain.Common.Exceptions;

/// <summary>
/// Subject area exception. Used for business rules that were not followed.
/// Should be handled globally and return 409 Conflict.
/// </summary>
public class DomainException : ExceptionWithStatusCode
{
    private const int DefaultStatusCode = 409;
    private const LogLevel DefaultLogLevel = LogLevel.Warning;

    public DomainException(string message)
        : base(message, DefaultStatusCode, logLevel: DefaultLogLevel, exposeMessage: true)
    {
    }

    public DomainException(string message, int? errorCode)
        : base(message, DefaultStatusCode, logLevel: DefaultLogLevel, errorCode: errorCode, exposeMessage: true)
    {
    }

    public DomainException(string message, Exception innerException)
        : base(message, DefaultStatusCode, innerException, logLevel: DefaultLogLevel, exposeMessage: true)
    {
    }
}
