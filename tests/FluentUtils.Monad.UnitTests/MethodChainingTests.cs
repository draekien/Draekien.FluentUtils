namespace FluentUtils.Monad.UnitTests;

using FluentAssertions;
using Monad.Extensions;
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
                                    .PipeAsync(_ => Task.FromResult(true))
                                    .TapAsync(boolTap)
                                    .PipeAsync(_ => "hello")
                                    .TapAsync(stringTap)
                                    .UnwrapAsync();

        result.Should().Be("hello");
        boolTap.Received(1).Invoke(Arg.Any<bool>());
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
    public void CanChainPipeWithAsyncExpressions()
    {
        string result = Result.Ok()
                              .Pipe(
                                   async _ =>
                                       await Task.FromResult(true))
                              .Pipe(_ => "hello")
                              .Match(x => x, _ => "bob");

        result.Should().Be("hello");
    }

    [Fact]
    public async Task CanChainPipeAsyncWithExpressionsThatReturnResults()
    {
        ResultType<string> result = await Result
                                         .BindAsync(
                                              () => Task.FromResult("hello"))
                                         .PipeAsync(
                                              hello =>
                                                  Result.Ok(hello + "World"))
                                         .PipeAsync(
                                              helloWorld => $"{helloWorld}");
    }
}
