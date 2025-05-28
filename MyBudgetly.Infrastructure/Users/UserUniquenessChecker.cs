using Microsoft.EntityFrameworkCore;
using MyBudgetly.Application.Interfaces;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Infrastructure.Users;

public class UserUniquenessChecker : IUserUniquenessChecker
{
    private readonly IApplicationDbContext _context;

    public UserUniquenessChecker(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        return !await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }
}