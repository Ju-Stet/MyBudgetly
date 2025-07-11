using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyBudgetly.API.Controllers.Base;
using MyBudgetly.Application.Users.Commands;
using MyBudgetly.Application.Users.Dto.Models;
using MyBudgetly.Application.Users.Queries;

namespace MyBudgetly.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class UserController(IMediator mediator) : MediatorController(mediator)
{
    /// <summary>
    /// Gets all users.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllUsersQuery.Message();
        var result = await SendMessage(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid userId)
    {
        var query = new GetUserQuery.Message { UserId = userId };
        var result = await SendMessage(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var command = new CreateUserCommand.Message { UserDto = dto };
        var newUserId = await SendMessage(command);

        return CreatedAtAction(nameof(GetById), new { userId = newUserId }, newUserId);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    [HttpPut("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid userId, [FromBody] UpdateUserDto dto)
    {
        var command = new UpdateUserCommand.Message { UserId = userId, UserDto = dto };
        var success = await SendMessage(command);

        return success ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var command = new DeleteUserCommand.Message { UserId = id };
        var result = await SendMessage(command);

        return result ? NoContent() : NotFound();
    }
}