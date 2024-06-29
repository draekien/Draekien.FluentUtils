namespace FluentUtils.Monad.UnitTests.Extensions;

using AutoFixture;
using FluentAssertions;
using Monad.Extensions;
using NSubstitute;

public class MapAsyncExtensionsTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task
        GivenAsyncOkResult_WhenInvokingMapAsync_ThenMapValueToNewType()
    {
        // Arrange
        Task<ResultType<Empty>> ok = Result.OkAsync();
        var expected = Substitute.For<ITestType>();

        // Act
        ResultType<ITestType> result =
            await ok.MapAsync((_, token) => Result.OkAsync(expected, token));

        // Assert
        result.Should().BeOfType<OkResultType<ITestType>>();
        result.Unwrap().Should().Be(expected);
    }

    [Fact]
    public async Task
        GivenAsyncErrorResult_WhenInvokingMapAsync_ThenMapErrorToNewType()
    {
        // Arrange
        var error = _fixture.Create<Error>();
        Task<ResultType<Empty>> errorResult = Result.ErrorAsync(error);
        ResultType<ITestType> expected =
            await Result.ErrorAsync<ITestType>(error);

        // Act
        ResultType<ITestType> result = await errorResult.MapAsync(
            (_, token) => Result.OkAsync(Substitute.For<ITestType>(), token)
        );

        // Assert
        result.Should().BeOfType<ErrorResultType<ITestType>>();
        result.As<ErrorResultType<ITestType>>()
           .Should()
           .Be(expected);
    }
}
