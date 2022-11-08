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

    [Fact]
    public async Task GivenIReverseMapFrom_WhenGettingUsers_ThenReturnUsers()
    {
        // ARRANGE
        HttpClient client = _factory.CreateClient();

        var expected = new List<UserDto>
        {
            new() { Id = 1, FullName = "Matthew Mercer" },
            new() { Id = 2, FullName = "Laura Bailey" },
        };

        // ACT
        var response = await client.GetFromJsonAsync<List<UserDto>>("Users");

        // ASSERT
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GivenIReverseMapFrom_WhenAddingUsers_ThenSucceed()
    {
        // ARRANGE
        HttpClient client = _factory.CreateClient();
        UserDto expected = new() { Id = 1, FullName = "Matthew Mercer" };


        // ACT
        HttpResponseMessage responseMessage = await client.PostAsJsonAsync("Users", expected, default);
        var response = await responseMessage.Content.ReadFromJsonAsync<UserDto>();

        // ASSERT
        response.Should().BeEquivalentTo(expected);
    }
}
