using MyBudgetly.Domain.Common;
using MyBudgetly.Domain.Utility;

namespace MyBudgetly.Domain.Users;

public class User : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; } = null!;
    public string? BackupEmail { get; set; } = null!;

    public User(string email)
    : base()
    {
        Email = email;
    }

    public User(Guid id, DateTime createdAt, string email)
        : base(id, createdAt)
    { 
        Email = email;
    }

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateBackupEmail(string? backupEmail) 
    {
        BackupEmail = backupEmail;
        UpdatedAt = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return ToStringUtility.ToString<User>(
            (nameof(FirstName), FirstName),
            (nameof(LastName), LastName));
    }
}