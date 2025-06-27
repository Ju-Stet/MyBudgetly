using MyBudgetly.Application.Users.Dto.Models;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Application.Users.Dto.Mappers;

public class UserDtoMapper
{
    public UserDto ToDto(User user)
    {
        return new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}