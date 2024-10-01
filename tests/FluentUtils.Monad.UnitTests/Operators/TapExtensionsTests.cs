namespace FluentUtils.Monad.UnitTests.Operators;

using FluentAssertions;
using Monad.Operators;
using NSubstitute;

public sealed class TapExtensionsTests
{
    [Fact]
    public void WhenTappingResult_ThenExecuteTap()
    {
        var tap = Substitute.For<Action<Empty>>();

        ResultType<Empty> result = Result.Ok().Tap(tap);

        tap.Received(1).Invoke(Arg.Any<Empty>());
        result.Should().BeOfType<OkResultType<Empty>>();
    }

    [Fact]
    public void WhenTappingResult_ThenAllowChaining()
    {
        var tap = Substitute.For<Action<Empty>>();

        ResultType<Empty> result = Result.Ok().Tap(tap).Tap(tap);

        tap.Received(2).Invoke(Arg.Any<Empty>());
        result.Should().BeOfType<OkResultType<Empty>>();
    }

    [Fact]
    public void GivenException_WhenTappingResult_ReturnErrorResult()
    {
        ResultType<Empty> result = Result.Ok().Tap(_ => throw new Exception());

        result.Should().BeOfType<ErrorResultType<Empty>>();
    }

    [Fact]
    public async Task WhenTappingAsyncResult_ThenExecuteTap()
    {
        var tap = Substitute.For<Func<Empty, Task>>();

        ResultType<Empty> result = await Result.OkAsync().Tap(tap);

        await tap.Received(1).Invoke(Arg.Any<Empty>());
        result.Should().BeOfType<OkResultType<Empty>>();
    }

    [Fact]
    public async Task WhenTappingAsyncResult_ThenAllowChaining()
    {
        var tap = Substitute.For<Func<Empty, Task>>();

        ResultType<Empty> result = await Result.OkAsync().Tap(tap).Tap(tap);

        await tap.Received(2).Invoke(Arg.Any<Empty>());
        result.Should().BeOfType<OkResultType<Empty>>();
    }

    [Fact]
    public async Task GivenException_WhenTappingAsyncResult_ReturnErrorResult()
    {
        ResultType<Empty> result =
            await Result.OkAsync().Tap(_ => throw new Exception());

        result.Should().BeOfType<ErrorResultType<Empty>>();
    }
}
