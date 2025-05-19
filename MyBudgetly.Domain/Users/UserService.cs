namespace MyBudgetly.Domain.Users;

public class UserService
{
    private readonly IUserUniquenessChecker _userUniquenessChecker;

    public UserService(IUserUniquenessChecker userUniquenessChecker)
    {
        _userUniquenessChecker = userUniquenessChecker;
    }

    public async Task<bool> CanChangeEmailAsync(string newEmail, CancellationToken cancellationToken = default)
    {
        return await _userUniquenessChecker.IsEmailUniqueAsync(newEmail, cancellationToken);
    }
}