namespace FluentUtils.Monad.UnitTests.Extensions;

using AutoFixture;
using FluentAssertions;
using Monad.Extensions;
using NSubstitute;

public class PipeAsyncExtensionsTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task
        GivenAsyncOkResult_WhenInvokingPipeAsync_ThenMapValueToNewType()
    {
        // Arrange
        Task<ResultType<Empty>> ok = Result.OkAsync();
        var expected = Substitute.For<ITestType>();

        // Act
        ResultType<ITestType> result =
            await ok.PipeAsync((_, _) => Task.FromResult(expected));

        // Assert
        result.Should().BeOfType<OkResultType<ITestType>>();
        result.Unwrap().Should().Be(expected);
    }

    [Fact]
    public async Task
        GivenAsyncErrorResult_WhenInvokingPipeAsync_ThenMapErrorToNewType()
    {
        // Arrange
        var error = _fixture.Create<Error>();
        Task<ResultType<Empty>> errorResult = Result.ErrorAsync(error);
        ResultType<ITestType> expected =
            await Result.ErrorAsync<ITestType>(error);

        // Act
        ResultType<ITestType> result = await errorResult.PipeAsync(
            (_, _) => Task.FromResult(Substitute.For<ITestType>())
        );

        // Assert
        result.Should().BeOfType<ErrorResultType<ITestType>>();
        result.As<ErrorResultType<ITestType>>()
           .Should()
           .Be(expected);
    }
}
