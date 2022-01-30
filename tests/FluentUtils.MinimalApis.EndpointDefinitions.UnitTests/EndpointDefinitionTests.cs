 using System.Collections.Generic;
 using System.Net.Http;
 using System.Net.Http.Json;
 using System.Threading.Tasks;
 using FluentAssertions;
 using FluentUtils.MinimalApis.EndpointDefinitions.Samples.Models;
 using Microsoft.AspNetCore.Mvc.Testing;
 using Xunit;

namespace FluentUtils.MinimalApis.EndpointDefinitions.UnitTests;

public class EndpointDefinitionTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public EndpointDefinitionTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GivenApplicationWithEndpointDefinitions_ThenRegisterThemCorrectly()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();

        // Act
        var response = await client.GetFromJsonAsync<IEnumerable<WeatherForecast>>("/weatherforecast");

        // Assert
        response.Should().NotBeNull();
    }
}
