using Microsoft.EntityFrameworkCore;
using MyBudgetly.Domain.Users;
using MyBudgetly.Domain.Users.Exceptions;
using MyBudgetly.Infrastructure.Persistence;

namespace MyBudgetly.Infrastructure.Users;

public class UserRepository(ApplicationDbContext context, UserDboMapper mapper) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserDboMapper _mapper = mapper;

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var dbUsers = await _context.Users.ToListAsync(cancellationToken);
        return dbUsers.Select(_mapper.ToDomain).ToList();
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbo = await _context.Users.FindAsync([id], cancellationToken);
        if (dbo is null)
            throw new UserNotFoundException(id);

        return _mapper.ToDomain(dbo);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var dbo = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        return dbo is null ? null : _mapper.ToDomain(dbo);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var dbo = _mapper.ToDbo(user);
        await _context.Users.AddAsync(dbo, cancellationToken);
    }

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbo = await _context.Users.FindAsync([id], cancellationToken);
        if (dbo is not null)
            _context.Users.Remove(dbo);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var dbo = await _context.Users.FindAsync([user.Id], cancellationToken)
                  ?? throw new UserNotFoundException(user.Id);

        _mapper.MapToExistingDbo(user, dbo);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}