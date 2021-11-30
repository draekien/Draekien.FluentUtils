using AutoMapper;

namespace FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;

public class WeatherForecastDto : IMapFrom<WeatherForecast>
{
    public DateTime Date { get; set; }
    public string? Summary { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF { get; set; }

    /// <inheritdoc />
    public void Mapping(Profile profile)
    {
        profile.CreateMap<WeatherForecast, WeatherForecastDto>()
               .ForMember(dest => dest.TemperatureF, opt => opt.MapFrom(src => 32 + (int)(src.TemperatureC / 0.5556)));
    }
}
