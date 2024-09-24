namespace FluentUtils.Monad.UnitTests;

using FluentAssertions;

public sealed class HttpStatusErrorTests
{
    [Fact]
    public void WhenInvokingToString_ThenReturnFormattedString()
    {
        HttpStatusCodeError error = new("code", "message");
        var result = error.ToString();
        result.Should().Be("code: message");
    }
}
