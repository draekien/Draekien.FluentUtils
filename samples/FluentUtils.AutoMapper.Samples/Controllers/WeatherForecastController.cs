using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FluentUtils.AutoMapper.Samples.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeatherForecastController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<WeatherForecastDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        GetForecastsQuery query = new();
        IEnumerable<WeatherForecastDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
