using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyBudgetly.API.Controllers.Base;
using MyBudgetly.Application.Users.Commands;
using MyBudgetly.Application.Users.Dto.Models;
using MyBudgetly.Application.Users.Queries;

namespace MyBudgetly.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IMediator mediator) : MediatorController(mediator)
{

    /// <summary>
    /// Gets user information by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>User details.</returns>
    [HttpGet("get-user")]
    public async Task<UserDto> GetUser([FromQuery][Required] Guid userId)
    {
        var query = new GetUserQuery.Message
        {
            UserId = userId
        };

        return await SendMessage(query);
    }
    /// <summary>
    /// Creates new User
    /// </summary>
    /// <param name="newUser">New user to create</param>
    /// <returns>ID of the created user</returns>
    [HttpPost("create-user")]
    public async Task<Guid> CreateUser(CreateUserDto newUser)
    {
        var createUserCommand = new CreateUserCommand.Message
        {
            UserDto = newUser
        };

        return await SendMessage(createUserCommand);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto user)
    {
        var updateUserCommand = new UpdateUserCommand.Message
        {
            UserId = id,
            UserDto = user
        };

        var result = await SendMessage(updateUserCommand);

        return result ? NoContent() : NotFound();
    }
}