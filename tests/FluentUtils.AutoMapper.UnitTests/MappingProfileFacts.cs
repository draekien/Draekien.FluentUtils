using AutoMapper;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;
using Xunit;

namespace FluentUtils.AutoMapper.UnitTests;

public class MappingProfileFacts
{
    [Fact]
    public void MappingConfigurationShouldBeValid()
    {
        // ARRANGE
        MapperConfiguration configuration = new(
            config =>
            {
                MappingProfile.AssembliesUsingIMapFrom = new[] { typeof(WeatherForecastDto).Assembly };
                MappingProfile profile = new();
                config.AddProfile(profile);
            }
        );

        // ASSERT
        configuration.AssertConfigurationIsValid();
    }
}
