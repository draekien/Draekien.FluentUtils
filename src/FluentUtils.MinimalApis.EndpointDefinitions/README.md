# FluentUtils.MinimalApis.EndpointDefinitions

## Example Usage


```c#
public class WeatherForecastEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(
            "/weatherforecast",
            (IWeatherForecastService weatherForecastService) =>
            {
                IEnumerable<WeatherForecast> result = weatherForecastService.GetAll();
                return Results.Ok(result);
            }
        ).Produces<IEnumerable<WeatherForecast>>();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastService, WeatherForecastService>();
    }
}
```
