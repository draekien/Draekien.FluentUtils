using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Commands;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FluentUtils.AutoMapper.Samples.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <inheritdoc />
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        GetUsersQuery query = new();
        List<UserDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddAsync([FromBody] UserDto user, CancellationToken cancellationToken)
    {
        AddUserCommand command = new() { User = user };
        UserDto result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
