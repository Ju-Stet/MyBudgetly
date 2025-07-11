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

    public void UpdateProfile(string? firstName, string? lastName, string? backupEmail)
    {
        var isChanged = false;

        if (!string.IsNullOrWhiteSpace(firstName) && FirstName != firstName)
        {
            FirstName = firstName;
            isChanged = true;
        }

        if (!string.IsNullOrWhiteSpace(lastName) && LastName != lastName)
        {
            LastName = lastName;
            isChanged = true;
        }

        if (backupEmail != null && BackupEmail != backupEmail)
        {
            BackupEmail = backupEmail;
            isChanged = true;
        }

        if (isChanged)
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public override string ToString()
    {
        return ToStringUtility.ToString<User>(
            (nameof(FirstName), FirstName),
            (nameof(LastName), LastName));
    }
}