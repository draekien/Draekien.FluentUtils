namespace FluentUtils.Monad.UnitTests;

using FluentAssertions;
using Monad.Operators;
using NSubstitute;

public sealed class MethodChainingTests
{
    [Fact]
    public void CanChainPipeWithTap()
    {
        var boolTap = Substitute.For<Action<bool>>();
        var stringTap = Substitute.For<Action<string>>();


        string result = Result.Ok(Substitute.For<ITestType>())
                              .Pipe(_ => true)
                              .Tap(boolTap)
                              .Pipe(_ => "hello")
                              .Tap(stringTap)
                              .Unwrap();

        result.Should().Be("hello");
        boolTap.Received(1).Invoke(Arg.Any<bool>());
        stringTap.Received(1).Invoke(Arg.Any<string>());
    }

    [Fact]
    public async Task CanChainPipeWithTapAsync()
    {
        var boolTap = Substitute.For<Action<bool>>();
        var stringTap = Substitute.For<Func<string, Task>>();


        string result = await Result.OkAsync(Substitute.For<ITestType>())
                                    .Pipe(_ => Task.FromResult(true))
                                    .Tap(boolTap)
                                    .Pipe(value => !value)
                                    .Tap(boolTap)
                                    .Pipe(_ => Task.FromResult("hello"))
                                    .Tap(stringTap)
                                    .Unwrap();

        result.Should().Be("hello");
        boolTap.Received(2).Invoke(Arg.Any<bool>());
        await stringTap.Received(1).Invoke(Arg.Any<string>());
    }

    [Fact]
    public void CanChainPipeWithEnsure()
    {
        bool result = Result.Ok()
                            .Pipe(_ => true)
                            .Ensure(x => x)
                            .Pipe(_ => "hello")
                            .Match(_ => true, _ => false);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task CanChainPipeWithAsyncExpressions()
    {
        string result = await Result.OkAsync()
                                    .Pipe(
                                         async _ =>
                                             await Task.FromResult(true))
                                    .Pipe(_ => "hello")
                                    .Match(x => x, _ => "bob");

        result.Should().Be("hello");
    }
}
