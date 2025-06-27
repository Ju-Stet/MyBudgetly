using MyBudgetly.Domain.Users;

namespace MyBudgetly.Application.Users;

public class UserApplicationService(IUserRepository userRepository, IUserUniquenessChecker uniquenessChecker)
{  

    public User CreateUser(
        string email, 
        string firstName, 
        string lastName, 
        string? backupEmail
        )
    {
        var user = new User(email);
        user.UpdateName(firstName, lastName);
        user.UpdateBackupEmail(backupEmail);
        return user;
    }
}