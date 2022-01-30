using FluentUtils.MinimalApis.EndpointDefinitions.Samples.Contracts;
using FluentUtils.MinimalApis.EndpointDefinitions.Samples.Models;
using FluentUtils.MinimalApis.EndpointDefinitions.Samples.Services;

namespace FluentUtils.MinimalApis.EndpointDefinitions.Samples.EndpointDefinitions;

public class WeatherForecastEndpointDefinition : IEndpointDefinition
{
    /// <inheritdoc />
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
    /// <inheritdoc />
    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastService, WeatherForecastService>();
    }
}
