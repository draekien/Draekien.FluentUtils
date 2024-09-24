namespace FluentUtils.Monad.UnitTests.Extensions;

using Monad.Extensions;
using NSubstitute;

public sealed class TapExtensionsTests
{
    [Fact]
    public void WhenTappingResult_ThenExecuteTap()
    {
        var tap = Substitute.For<Action<Empty>>();

        Result.Ok().Tap(tap);

        tap.Received(1).Invoke(Arg.Any<Empty>());
    }

    [Fact]
    public void WhenTappingResult_ThenAllowChaining()
    {
        var tap = Substitute.For<Action<Empty>>();

        Result.Ok().Tap(tap).Tap(tap);

        tap.Received(2).Invoke(Arg.Any<Empty>());
    }
}
