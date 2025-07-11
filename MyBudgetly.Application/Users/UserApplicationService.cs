using MyBudgetly.Domain.Users;

namespace MyBudgetly.Application.Users;

public class UserApplicationService
{
    public User CreateUser(string email, string firstName, string lastName, string? backupEmail)
    {
        var user = new User(email);
        user.UpdateProfile(firstName, lastName, backupEmail);
        return user;
    }
}