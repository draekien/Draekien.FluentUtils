namespace FluentUtils.Monad.UnitTests.Extensions;

using FluentAssertions;
using Monad.Extensions;
using NSubstitute;

public class MatchExtensionsTests
{
    [Fact]
    public void GivenEmptyOkResult_WhenInvokingMatch_ThenInvokeOkHandler()
    {
        // Arrange
        var okHandler = Substitute.For<Func<ITestType>>();
        var errorHandler = Substitute.For<Func<Error, ITestType>>();

        ResultType<Empty> okResult = Result.Ok();

        // Act
        ITestType result = okResult.Match(okHandler, errorHandler);

        // Assert
        result.Should().NotBeNull();
        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public void GivenEmptyErrorResult_WhenInvokingMatch_ThenInvokeErrorHandler()
    {
        // Arrange
        var okHandler = Substitute.For<Func<ITestType>>();
        var errorHandler = Substitute.For<Func<Error, ITestType>>();

        ResultType<Empty> errorResult = Result.Error("test");

        // Act
        ITestType result = errorResult.Match(okHandler, errorHandler);

        // Assert
        result.Should().NotBeNull();
        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }

    [Fact]
    public void
        GivenOkResult_AndVoidHandler_WhenInvokingMatch_ThenInvokeOkHandler()
    {
        // Arrange
        var okHandler = Substitute.For<Action<ITestType>>();
        var errorHandler = Substitute.For<Action<Error>>();

        ResultType<ITestType> okResult = Result.Ok(Substitute.For<ITestType>());

        // Act
        okResult.Match(okHandler, errorHandler);

        // Assert
        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public void
        GivenErrorResult_AndVoidHandler_WhenInvokingMatch_ThenInvokeErrorHandler()
    {
        // Arrange
        var okHandler = Substitute.For<Action<ITestType>>();
        var errorHandler = Substitute.For<Action<Error>>();

        ResultType<ITestType> errorResult = Result.Error<ITestType>("test");

        // Act
        errorResult.Match(okHandler, errorHandler);

        // Assert
        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }

    [Fact]
    public void
        GivenEmptyOkResult_AndVoidHandler_WhenInvokingMatch_ThenInvokeOkHandler()
    {
        // Arrange
        var okHandler = Substitute.For<Action>();
        var errorHandler = Substitute.For<Action<Error>>();

        ResultType<Empty> okResult = Result.Ok();

        // Act
        okResult.Match(okHandler, errorHandler);

        // Assert
        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public void
        GivenEmptyErrorResult_AndVoidHandler_WhenInvokingMatch_ThenInvokeErrorHandler()
    {
        // Arrange
        var okHandler = Substitute.For<Action>();
        var errorHandler = Substitute.For<Action<Error>>();

        ResultType<Empty> errorResult = Result.Error("test");

        // Act
        errorResult.Match(okHandler, errorHandler);

        // Assert
        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }
}
