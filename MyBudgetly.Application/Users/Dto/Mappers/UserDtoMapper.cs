using MyBudgetly.Application.Users.Dto.Models;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Application.Users.Dto.Mappers;

public class UserDtoMapper
{
    public UserDto ToDto(User user)
    {
        return new UserDto
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public List<UserDto> ToDtoList(IEnumerable<User> users)
    {
        return users
            .Select(u => ToDto(u))
            .ToList();
    }
}