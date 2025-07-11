using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyBudgetly.API.Controllers.Base;
using MyBudgetly.Application.Common.Models;
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
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetUserQuery.Message { UserId = id };
        var result = await SendMessage(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var command = new CreateUserCommand.Message { UserDto = dto };
        var newUserId = await SendMessage(command);

        var message = $"User created successfully with ID: {newUserId}";
        var response = ApiResponse<Guid>.SuccessResponse(newUserId, 200, message);

        return CreatedAtAction(nameof(GetById), new { id = newUserId }, response);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Update(Guid id, [FromBody] UpdateUserDto user)
    {
        var command = new UpdateUserCommand.Message
        {
            UserId = id,
            UserDto = user
        };

        var result = await SendMessage(command);

        if (!result)
            return NotFound(ApiResponse<bool>.Failure("User not found.", 404));

        return Ok(ApiResponse<bool>.SuccessResponse(true, 200, "User updated successfully."));
    }

    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
    {
        var command = new DeleteUserCommand.Message { UserId = id };
        var result = await SendMessage(command);

        if (!result)
            return NotFound(ApiResponse<bool>.Failure("User not found.", 404));

        return Ok(ApiResponse<bool>.SuccessResponse(true, 200, "User deleted successfully."));
    }
}