using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FluentUtils.FromCompositeAttribute.Samples.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FluentUtils.FromCompositeAttribute.UnitTests;

public class FromCompositeAttributeTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public FromCompositeAttributeTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GivenIdInRouteAndDataInBody_ThenBindIdCorrectly()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        UserDto user = new() { FirstName = "John", LastName = "Smith" };
        ExampleRequest expected = new()
        {
            Id = 1,
            Replace = true,
            User = user
        };

        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("example/1?replace=true", user, default);

        // Assert
        response.EnsureSuccessStatusCode();
        var actual = await response.Content.ReadFromJsonAsync<ExampleRequest>();
        actual.Should().BeEquivalentTo(expected);
    }
}
