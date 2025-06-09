namespace MyBudgetly.Domain.Users.Exceptions;

public class UserNotFoundException : Exception
{
    public Guid UserId { get; }

    public UserNotFoundException(Guid userId)
    {
        UserId = userId;
    }
}
