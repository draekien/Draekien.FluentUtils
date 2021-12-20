using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FluentUtils.MediatR.Pagination;
using FluentUtils.MediatR.Samples;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FluentUtils.MediatR.UnitTests;

public class MediatorApiControllerFacts : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public MediatorApiControllerFacts(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GivenOffsetIsZero_WhenGettingWeatherForecasts_ThenReturnCorrectLinks()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        Links expected = new()
        {
            Self = new Uri("/WeatherForecast?Limit=10&Offset=0", UriKind.Relative),
            Next = new Uri("/WeatherForecast?Limit=10&Offset=10", UriKind.Relative)
        };

        // Act
        var response = await client.GetFromJsonAsync<PaginatedResponse<WeatherForecast>>("WeatherForecast?offset=0&limit=10");

        // Assert
        response.Should().NotBeNull();
        response!.Links.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GivenOffsetGreaterThanLimit_WhenGettingWeatherForecasts_ThenReturnCorrectLinks()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        Links expected = new()
        {
            Self = new Uri("/WeatherForecast?Limit=10&Offset=10", UriKind.Relative),
            Next = new Uri("/WeatherForecast?Limit=10&Offset=20", UriKind.Relative),
            Previous = new Uri("/WeatherForecast?Limit=10&Offset=0", UriKind.Relative)
        };

        // Act
        var response = await client.GetFromJsonAsync<PaginatedResponse<WeatherForecast>>("WeatherForecast?offset=10&limit=10");

        // Assert
        response.Should().NotBeNull();
        response!.Links.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GivenLimitLessThanZero_WhenGettingWeatherForecasts_ThenLimitShouldBeMinValue()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        Links expected = new()
        {
            Self = new Uri("/WeatherForecast?Limit=10&Offset=0", UriKind.Relative),
            Next = new Uri("/WeatherForecast?Limit=10&Offset=10", UriKind.Relative)
        };

        // Act
        var response = await client.GetFromJsonAsync<PaginatedResponse<WeatherForecast>>("WeatherForecast?offset=0&limit=-10");

        // Assert
        response.Should().NotBeNull();
        response!.Links.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GivenLimitGreaterThanOneHundred_WhenGettingWeatherForecasts_ThenLimitShouldBeMaxValue()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        Links expected = new()
        {
            Self = new Uri("/WeatherForecast?Limit=100&Offset=0", UriKind.Relative)
        };

        // Act
        var response = await client.GetFromJsonAsync<PaginatedResponse<WeatherForecast>>("WeatherForecast?offset=0&limit=200");

        // Assert
        response.Should().NotBeNull();
        response!.Links.Should().BeEquivalentTo(expected);
    }
}
