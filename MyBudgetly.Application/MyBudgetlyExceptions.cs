using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyBudgetly.Domain.Common.Exceptions;

namespace MyBudgetly.Application;

public static class MyBudgetlyExceptions
{
    public static ExceptionWithStatusCode GetUserNotFoundException(Guid userId)
    {
        return new ExceptionWithStatusCode(
            message: $"User with id {userId} was not found.",
            statusCode: StatusCodes.Status404NotFound,
            logLevel: LogLevel.Warning
        );
    }

    public static ExceptionWithStatusCode GetEmailAlreadyInUseException(string email) =>
        new(
            message: $"Email '{email}' is already in use.",
            statusCode: StatusCodes.Status409Conflict,
            errorCode: 1001,
            logLevel: LogLevel.Warning
        );
}