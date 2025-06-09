using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetly.API.Controllers.Base;

[ApiController]
public abstract class MediatorController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    protected async Task<T> SendMessage<T>(IRequest<T> message)
    {
        return await _mediator.Send(message, HttpContext.RequestAborted);
    }
}

