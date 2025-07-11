namespace MyBudgetly.Domain.Users;

public interface IUserUniquenessChecker
{
    Task<bool> IsEmailUsedByAnotherUserAsync(Guid? currentUserId, string email, CancellationToken ct = default);
}