using FluentUtils.MediatR.Pagination;
using FluentUtils.MediatR.Samples.Core.Queries;
using Microsoft.AspNetCore.Mvc;

namespace FluentUtils.MediatR.Samples.Controllers;

public class WeatherForecastController : MediatorApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<WeatherForecast>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] GetWeatherForecastQuery query, CancellationToken cancellationToken)
    {
        PaginatedResponse<WeatherForecast> response = await Mediator.Send(query, cancellationToken);
        response.Links = CreatePaginationLinks(nameof(Get), query, response);
        return Ok(response);
    }
}
