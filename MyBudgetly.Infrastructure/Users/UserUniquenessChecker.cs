using Microsoft.EntityFrameworkCore;
using MyBudgetly.Domain.Users;
using MyBudgetly.Infrastructure.Persistence.Abstractions;

namespace MyBudgetly.Infrastructure.Users;

public class UserUniquenessChecker(IApplicationDbContext context) : IUserUniquenessChecker
{
    public async Task<bool> IsEmailUsedByAnotherUserAsync(Guid? currentUserId, string email, CancellationToken ct = default)
    {
        return await context.Users.AnyAsync(u =>
            (!currentUserId.HasValue || u.Id != currentUserId.Value) &&
            (u.Email == email || u.BackupEmail == email),
            ct);
    }
}