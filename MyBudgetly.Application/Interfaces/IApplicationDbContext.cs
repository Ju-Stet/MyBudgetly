using Microsoft.EntityFrameworkCore;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
