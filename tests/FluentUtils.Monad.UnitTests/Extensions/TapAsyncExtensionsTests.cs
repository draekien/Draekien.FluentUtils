namespace FluentUtils.Monad.UnitTests.Extensions;

using FluentAssertions;
using Monad.Extensions;
using NSubstitute;

public sealed class TapAsyncExtensionsTests
{
    [Fact]
    public async Task WhenTappingResult_ThenExecuteTap()
    {
        var tap = Substitute.For<Func<Empty, Task>>();

        await Result.OkAsync().TapAsync(tap);

        await tap.Received(1).Invoke(Arg.Any<Empty>());
    }

    [Fact]
    public async Task WhenTappingResult_ThenAllowChaining()
    {
        var tap = Substitute.For<Func<Empty, Task>>();

        await Result.OkAsync().TapAsync(tap).TapAsync(tap);

        await tap.Received(2).Invoke(Arg.Any<Empty>());
    }

    [Fact]
    public async Task GivenException_WhenTappingResult_ReturnErrorResult()
    {
        ResultType<Empty> result =
            await Result.OkAsync().TapAsync(_ => throw new Exception());

        result.Should().BeOfType<ErrorResultType<Empty>>();
    }
}
