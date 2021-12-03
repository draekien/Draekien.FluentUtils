using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FluentUtils.AutoMapper.UnitTests;

public class AddAutoMapperProfileFacts : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AddAutoMapperProfileFacts(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ApplicationStarts()
    {
        // ARRANGE
        HttpClient client = _factory.CreateClient();

        // ACT
        var response = await client.GetFromJsonAsync<IEnumerable<WeatherForecastDto>>("WeatherForecast");

        // ASSERT
        response.Should().NotBeNull();
    }
}
