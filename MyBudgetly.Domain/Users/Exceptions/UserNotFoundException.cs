using Microsoft.AspNetCore.Http;
using MyBudgetly.Domain.Common.Exceptions;

namespace MyBudgetly.Domain.Users.Exceptions;

public class UserNotFoundException : ExceptionWithStatusCode
{
    public Guid UserId { get; }

    public UserNotFoundException(Guid userId)
        : base($"User with ID {userId} was not found.", StatusCodes.Status404NotFound, exposeMessage: true)
    {
        UserId = userId;
    }
}