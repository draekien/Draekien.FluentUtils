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
    public void
        GivenEmptyOkResult_AndFuncHandler_WhenPerformingMatch_ThenExecuteOkHandler()
    {
        var okHandler = Substitute.For<Func<Empty>>();
        var errorHandler = Substitute.For<Func<Error, Empty>>();
        ResultType<Empty> result = Result.Ok();

        result.Match(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public void
        GivenEmptyErrorResult_AndSingleParamFuncHandler_WhenPerformingMatch_ThenExecuteErrorHandler()
    {
        var okHandler = Substitute.For<Func<Empty>>();
        var errorHandler = Substitute.For<Func<Error, Empty>>();
        ResultType<Empty> result = Result.Error("test");

        result.Match(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }

    [Fact]
    public void
        GivenEmptyOkResult_AndActionHandlerWithNoParam_WhenPerformingMatch_ThenExecuteOkHandler()
    {
        var okHandler = Substitute.For<Action>();
        var errorHandler = Substitute.For<Action<Error>>();
        ResultType<Empty> result = Result.Ok();

        result.Match(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public void
        GivenEmptyErrorResult_AndActionHandlerWithNoParam_WhenPerformingMatch_ThenExecuteErrorHandler()
    {
        var okHandler = Substitute.For<Action>();
        var errorHandler = Substitute.For<Action<Error>>();
        ResultType<Empty> result = Result.Error("test");

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
        GivenErrorResult_AndActionHandler_WhenPerformingMatchAsync_ThenExecuteErrorHandler()
    {
        var okHandler = Substitute.For<Func<ITestType, ITestType>>();
        var errorHandler = Substitute.For<Func<Error, ITestType>>();
        Task<ResultType<ITestType>> resultTask =
            Task.FromResult(Result.Error<ITestType>("Test"));

        await resultTask.MatchAsync(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
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

    [Fact]
    public async Task
        GivenEmptyErrorResult_AndActionHandler_WhenPerformingMatchAsync_ThenExecuteErrorHandler()
    {
        var okHandler = Substitute.For<Func<Empty, Empty>>();
        var errorHandler = Substitute.For<Func<Error, Empty>>();
        Task<ResultType<Empty>> resultTask =
            Task.FromResult(Result.Error("test"));

        await resultTask.MatchAsync(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }

    [Fact]
    public async Task
        GivenEmptyOkResult_AndFuncHandlerWithNoParams_WhenPerformingMatchAsync_ThenExecuteOkHandler()
    {
        var okHandler = Substitute.For<Func<Empty>>();
        var errorHandler = Substitute.For<Func<Error, Empty>>();
        Task<ResultType<Empty>> resultTask =
            Task.FromResult(Result.Ok());

        await resultTask.MatchAsync(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task
        GivenEmptyErrorResult_AndFuncHandlerWithNoParams_WhenPerformingMatchAsync_ThenExecuteErrorHandler()
    {
        var okHandler = Substitute.For<Func<Empty>>();
        var errorHandler = Substitute.For<Func<Error, Empty>>();
        Task<ResultType<Empty>> resultTask =
            Task.FromResult(Result.Error("test"));

        await resultTask.MatchAsync(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }

    [Fact]
    public async Task
        GivenEmptyOkResult_AndActionHandlerWithNoParams_WhenPerformingMatchAsync_ThenExecuteOkHandler()
    {
        var okHandler = Substitute.For<Action>();
        var errorHandler = Substitute.For<Action<Error>>();
        Task<ResultType<Empty>> resultTask =
            Task.FromResult(Result.Ok());

        await resultTask.MatchAsync(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task
        GivenEmptyErrorResult_AndActionHandlerWithNoParams_WhenPerformingMatchAsync_ThenExecuteErrorHandler()
    {
        var okHandler = Substitute.For<Action>();
        var errorHandler = Substitute.For<Action<Error>>();
        Task<ResultType<Empty>> resultTask =
            Task.FromResult(Result.Error("test"));

        await resultTask.MatchAsync(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }

    [Fact]
    public async Task
        GivenOkResult_AndFuncHandlerWithCancellation_WhenPerformingMatchAsync_ThenExecuteOkHandler()
    {
        var okHandler = Substitute
           .For<Func<ITestType, CancellationToken, Task<ITestType>>>();
        var errorHandler = Substitute
           .For<Func<Error, CancellationToken, Task<ITestType>>>();
        var value = Substitute.For<ITestType>();
        Task<ResultType<ITestType>> resultTask =
            Task.FromResult(Result.Ok(value));

        await resultTask.MatchAsync(
            okHandler,
            errorHandler,
            CancellationToken.None
        );

        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public void GivenOkResult_WhenPerformingMap_ThenReturnMappedValue()
    {
        ResultType<Empty> result = Result.Ok();

        List<string> output =
            result.Map(_ => Result.Ok(new List<string>())).Unwrap();
        output.Should().BeEquivalentTo(new List<string>());
    }

    [Fact]
    public void GivenErrorResult_WhenPerformingMap_ThenReturnErrorResult()
    {
        ResultType<Empty> result = Result.Error("test");

        ResultType<List<string>> output =
            result.Map(_ => Result.Ok(new List<string>()));

        output.Should().BeOfType<ErrorResultType<List<string>>>();
    }
}
