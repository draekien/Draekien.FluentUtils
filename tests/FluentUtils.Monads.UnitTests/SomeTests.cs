namespace FluentUtils.Monads.UnitTests;

using FluentAssertions;

public sealed class SomeTests
{
    [Fact]
    public void WhenCreatingSome_ThenReturnsSome()
    {
        IOption<int> some = Option.Some(1);

        some.IsSome.Should().BeTrue();
        some.IsNone.Should().BeFalse();
        some.Unwrap().Should().Be(1);
    }
}
