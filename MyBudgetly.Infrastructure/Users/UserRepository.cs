using Microsoft.EntityFrameworkCore;
using MyBudgetly.Domain.Users;
using MyBudgetly.Domain.Users.Exceptions;
using MyBudgetly.Infrastructure.Persistence;

namespace MyBudgetly.Infrastructure.Users;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    { 
        var user = await _context.Users.FindAsync([id], cancellationToken: cancellationToken) ?? throw new UserNotFoundException(id);
        return user;
    }        

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await _context.Users.AddAsync(user, cancellationToken);

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user != null)
        {
            _context.Users.Remove(user);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}