using Microsoft.EntityFrameworkCore;
using MyBudgetly.Infrastructure.Persistence.Models;

namespace MyBudgetly.Infrastructure.Persistence.Abstractions;

public interface IApplicationDbContext
{
    DbSet<UserDbo> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
