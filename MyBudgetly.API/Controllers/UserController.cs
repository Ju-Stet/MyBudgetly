using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyBudgetly.API.Controllers.Base;
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
}