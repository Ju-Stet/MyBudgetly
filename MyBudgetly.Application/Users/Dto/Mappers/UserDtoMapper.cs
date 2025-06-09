using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
