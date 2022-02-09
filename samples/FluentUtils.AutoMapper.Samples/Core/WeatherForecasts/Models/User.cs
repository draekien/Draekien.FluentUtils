namespace FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
}
