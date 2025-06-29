using Microsoft.EntityFrameworkCore;
using MyBudgetly.Application.Interfaces;
using MyBudgetly.Domain.Users;

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