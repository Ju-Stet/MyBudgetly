using MyBudgetly.Domain.Users;
using MyBudgetly.Infrastructure.Persistence.Models;

namespace MyBudgetly.Infrastructure.Users;

public class UserDboMapper
{
    public UserDbo ToDbo(User user)
    {
        var dbo = new UserDbo
        {
            Id = user.Id,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            BackupEmail = user.BackupEmail
        };

        return dbo;
    }

    public User ToDomain(UserDbo dbo)
    {
        return new User(dbo.Id, dbo.CreatedAt, dbo.Email)
        {
            UpdatedAt = dbo.UpdatedAt,
            FirstName = dbo.FirstName,
            LastName = dbo.LastName,
            BackupEmail = dbo.BackupEmail
        };
    }

    public void MapToExistingDbo(User user, UserDbo dbo)
    {
        dbo.FirstName = user.FirstName;
        dbo.LastName = user.LastName;
        dbo.BackupEmail = user.BackupEmail;
        dbo.UpdatedAt = user.UpdatedAt ?? DateTime.UtcNow;
    }
}