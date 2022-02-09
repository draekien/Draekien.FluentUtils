namespace FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;

public class UserDto : IReverseMapFrom<User>
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
}
