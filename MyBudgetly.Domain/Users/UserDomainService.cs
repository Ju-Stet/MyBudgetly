namespace MyBudgetly.Domain.Users;

public class UserDomainService(IUserUniquenessChecker userUniquenessChecker)
{
    private readonly IUserUniquenessChecker _userUniquenessChecker = userUniquenessChecker;
    public async Task<bool> CanChangeEmailAsync(string newEmail, CancellationToken cancellationToken = default)
    {
        return await _userUniquenessChecker.IsEmailUniqueAsync(newEmail, cancellationToken);
    }
}