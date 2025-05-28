namespace MyBudgetly.Domain.Users;

public interface IUserUniquenessChecker
{
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
}