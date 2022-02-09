using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;

namespace FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Contracts;

public interface IUserService
{
    Task<List<User>> ListAsync(CancellationToken cancellationToken);
    Task<User> AddAsync(User user, CancellationToken cancellationToken);
}
