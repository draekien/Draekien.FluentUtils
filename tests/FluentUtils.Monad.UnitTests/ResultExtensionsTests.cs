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
    public void GivenOkResult_WhenPerformingMatch_ThenExecuteTheOkHandler()
    {
        var okHandler = Substitute.For<Action<ITestType>>();
        var errorHandler = Substitute.For<Action<Error>>();
        var value = Substitute.For<ITestType>();
        ResultType<ITestType> result = Result.Ok(value);

        result.Match(okHandler, errorHandler);

        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }
}
