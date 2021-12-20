# FluentUtils.MediatR.Pagination.AspNetCore

## Example Usage

Have your controller inherit from `MediatorApiController`

```c#
public class MyController : MediatorApiController
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
```
