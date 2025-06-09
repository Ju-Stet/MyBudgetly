using MyBudgetly.Domain.Users;

namespace MyBudgetly.Application.Users;

public class UserApplicationService(IUserRepository userRepository, IUserUniquenessChecker uniquenessChecker)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserUniquenessChecker _uniquenessChecker = uniquenessChecker;

    public async Task<bool> UpdateNameAsync(Guid userId, string newFirstName, string newLastName, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return false;

        user.UpdateName(newFirstName, newLastName);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _uniquenessChecker.IsEmailUniqueAsync(email, cancellationToken);
    }
}