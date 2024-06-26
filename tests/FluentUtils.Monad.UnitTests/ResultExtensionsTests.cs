namespace FluentUtils.Monad.UnitTests;

using AutoFixture;
using FluentAssertions;
using NSubstitute;

public class ResultExtensionsTests
{
    private readonly Fixture _fixture;

    public ResultExtensionsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void GivenEmptyOkResultType_WhenUnwrapping_ThenReturnEmptyValue()
    {
        ResultType<Empty> result = Result.Ok();

        result.Unwrap().Should().BeEquivalentTo(new Empty());
    }

    [Fact]
    public void GivenGenericOkResultType_WhenUnwrapping_ThenReturnGenericType()
    {
        var value = Substitute.For<ITestType>();
        ResultType<ITestType> result = Result.Ok(value);

        result.Unwrap().Should().Be(value);
    }

    [Fact]
    public void
        GivenErrorResultType_WhenUnwrapping_ThenThrowUnwrapPanicException()
    {
        var error = _fixture.Create<Error>();
        ResultType<Empty> result = Result.Error(error);

        result.Invoking(x => x.Unwrap())
           .Should()
           .Throw<UnwrapPanicException>()
           .Where(x => x.Message.Equals(error.ToString()));
    }

    [Fact]
    public void
        GivenOkResult_AndActionHandler_WhenPerformingMatch_ThenExecuteTheOkHandler()
    {
        var okHandler = Substitute.For<Action<ITestType>>();
        var errorHandler = Substitute.For<Action<Error>>();
        var value = Substitute.For<ITestType>();
        ResultType<ITestType> result = Result.Ok(value);

        result.Match(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public void
        GivenErrorResult_AndActionHandler_WhenPerformingMatch_ThenExecuteErrorHandler()
    {
        var okHandler = Substitute.For<Action<ITestType>>();
        var errorHandler = Substitute.For<Action<Error>>();
        ResultType<ITestType> result = Result.Error<ITestType>(string.Empty);

        result.Match(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }

    [Fact]
    public void
        GivenOkResult_WithFuncHandler_WhenPerformingMatch_ThenExecuteOkHandler()
    {
        var okHandler = Substitute.For<Func<ITestType, ITestType>>();
        var errorHandler = Substitute.For<Func<Error, ITestType>>();
        var value = Substitute.For<ITestType>();
        ResultType<ITestType> ok = Result.Ok(value);

        ok.Match(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public void
        GivenErrorResult_AndFuncHandler_WhenPerformingMatch_ThenExecuteErrorHandler()
    {
        var okHandler = Substitute.For<Func<ITestType, ITestType>>();
        var errorHandler = Substitute.For<Func<Error, ITestType>>();
        ResultType<ITestType> result = Result.Error<ITestType>(string.Empty);

        result.Match(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }

    [Fact]
    public void
        GivenEmptyOkResult_WithFuncHandler_WhenPerformingMatch_ThenExecuteOkHandler()
    {
        var okHandler = Substitute.For<Func<Empty, Empty>>();
        var errorHandler = Substitute.For<Func<Error, Empty>>();
        ResultType<Empty> ok = Result.Ok();

        ok.Match(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public void
        GivenEmptyErrorResult_AndFuncHandler_WhenPerformingMatch_ThenExecuteErrorHandler()
    {
        var okHandler = Substitute.For<Func<Empty, Empty>>();
        var errorHandler = Substitute.For<Func<Error, Empty>>();
        ResultType<Empty> result = Result.Error<Empty>(string.Empty);

        result.Match(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }

    [Fact]
    public async Task
        GivenOkResult_AndActionHandler_WhenPerformingMatchAsync_ThenExecuteOkHandler()
    {
        var okHandler = Substitute.For<Func<ITestType, ITestType>>();
        var errorHandler = Substitute.For<Func<Error, ITestType>>();
        var value = Substitute.For<ITestType>();
        Task<ResultType<ITestType>> resultTask =
            Task.FromResult(Result.Ok(value));

        await resultTask.MatchAsync(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task
        GivenEmptyOkResult_AndActionHandler_WhenPerformingMatchAsync_ThenExecuteOkHandler()
    {
        var okHandler = Substitute.For<Func<Empty, Empty>>();
        var errorHandler = Substitute.For<Func<Error, Empty>>();
        Task<ResultType<Empty>> resultTask =
            Task.FromResult(Result.Ok());

        await resultTask.MatchAsync(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }
}
