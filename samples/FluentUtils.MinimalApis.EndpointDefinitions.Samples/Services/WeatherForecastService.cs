using FluentUtils.MinimalApis.EndpointDefinitions.Samples.Contracts;
using FluentUtils.MinimalApis.EndpointDefinitions.Samples.Models;

namespace FluentUtils.MinimalApis.EndpointDefinitions.Samples.Services;

public class WeatherForecastService
    : IWeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public IEnumerable<WeatherForecast> GetAll()
    {
        return Enumerable.Range(1, 5).Select(
                             index => new WeatherForecast
                             {
                                 Date = DateTime.Now.AddDays(index),
                                 TemperatureC = Random.Shared.Next(-20, 55),
                                 Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                             }
                         )
                         .ToArray();
    }
}
