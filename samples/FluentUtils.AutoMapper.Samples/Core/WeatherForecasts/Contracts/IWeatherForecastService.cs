using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;

namespace FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Contracts;

public interface IWeatherForecastService
{
    Task<IEnumerable<WeatherForecast>> GetCurrentForecastAsync(CancellationToken cancellationToken);
}
