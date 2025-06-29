using MyBudgetly.Domain.Common.Exceptions;

namespace MyBudgetly.Domain.Users;

/// <summary>
/// Domain service for business rules related to users.
/// Rule: no e-mail (primary or backup) should be the same 
/// as the e-mails of other users.
/// </summary>
public class UserDomainService
{
    private readonly IUserUniquenessChecker _checker;

    public UserDomainService(IUserUniquenessChecker checker) => _checker = checker;

    /// <summary>
    /// Checks whether a new user can register with this e-mail address.
    /// </summary>
    public async Task<bool> CanUseEmailAsync(string email, CancellationToken ct = default)
    {
        // null in the first argument - because there is no user yet.
        return !await _checker.IsEmailUsedByAnotherUserAsync(null, email, ct);
    }

    /// <summary>
    /// Updates the user profile with a backup e-mail check.
    /// Throws a DomainException if the e-mail is already occupied 
    /// by another user or is equal to the user's own primary e-mail.
    /// </summary>
    public async Task UpdateUserAsync(
        User user,
        string? firstName,
        string? lastName,
        string? backupEmail,
        CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(backupEmail))
        {
            // 1) must differ from other users' e-mails
            var usedBySomeoneElse =
                await _checker.IsEmailUsedByAnotherUserAsync(user.Id, backupEmail, ct);

            if (usedBySomeoneElse)
                throw new DomainException("Backup email is already used by another user.");

            // 2) must differ from own primary e-mail
            if (backupEmail.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
                throw new DomainException("Backup email must differ from the primary email.");
        }

        user.UpdateProfile(firstName, lastName, backupEmail);
    }
}