using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Contracts;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;

namespace FluentUtils.AutoMapper.Samples.Infrastructure.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private static readonly string[] Summaries =
    {
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    };

    /// <inheritdoc />
    public async Task<IEnumerable<WeatherForecast>> GetCurrentForecastAsync(CancellationToken cancellationToken)
    {
        IEnumerable<int> days = Enumerable.Range(1, 5);
        IEnumerable<WeatherForecast> forecasts = days.Select(
            index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }
        );

        await Task.Delay(1000, cancellationToken);

        return forecasts;
    }
}
