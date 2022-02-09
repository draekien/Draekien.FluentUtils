using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Contracts;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;

namespace FluentUtils.AutoMapper.Samples.Infrastructure.Services;

public class UserService : IUserService
{
    /// <inheritdoc />
    public async Task<List<User>> ListAsync(CancellationToken cancellationToken)
    {
        return await Task.FromResult(
            new List<User>
            {
                new() { Id = 1, FullName = "Matthew Mercer" },
                new() { Id = 2, FullName = "Laura Bailey" },
            }
        );
    }
    /// <inheritdoc />
    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        return await Task.FromResult(user);
    }
}
