using FluentUtils.MinimalApis.EndpointDefinitions.Samples.Models;

namespace FluentUtils.MinimalApis.EndpointDefinitions.Samples.Contracts;

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> GetAll();
}
