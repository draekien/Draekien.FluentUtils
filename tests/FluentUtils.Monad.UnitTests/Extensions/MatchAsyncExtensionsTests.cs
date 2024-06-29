namespace FluentUtils.Monad.UnitTests.Extensions;

using AutoFixture;
using Exceptions;
using FluentAssertions;
using Monad.Extensions;
using NSubstitute;

public class MatchAsyncExtensionsTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task
        GivenErrorResultForValue_WhenInvokingMatchAsync_ThenInvokeErrorHandler()
    {
        // Arrange
        var okHandler =
            Substitute.For<Func<ITestType, CancellationToken, Task<Empty>>>();
        var errorHandler = Substitute.For<
            Func<Error, CancellationToken, Task<Empty>>>();

        var error = _fixture.Create<Error>();

        Task<ResultType<ITestType>> errorResult =
            Result.ErrorAsync<ITestType>(error);

        // Act
        Empty result = await errorResult.MatchAsync(
            okHandler,
            errorHandler,
            CancellationToken.None
        );

        // Assert
        result.Should().BeOfType<Empty>();
        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }

    [Fact]
    public async Task
        GivenCustomResultTypeForValue_WhenInvokingMatchAsync_ThenThrowUnsupportedResultTypeException()
    {
        // Arrange
        var okHandler =
            Substitute.For<Func<ITestType, CancellationToken, Task<Empty>>>();
        var errorHandler = Substitute.For<
            Func<Error, CancellationToken, Task<Empty>>>();

        Task<ResultType<ITestType>> errorResult =
            Task.FromResult<ResultType<ITestType>>(
                new CustomResultType<ITestType>()
            );

        // Act + Assert
        await errorResult.Invoking(
                x => x.MatchAsync(
                    okHandler,
                    errorHandler,
                    CancellationToken.None
                )
            )
           .Should()
           .ThrowAsync<UnsupportedResultTypeException<ITestType>>()
           .WithMessage(
                $"The result type '{typeof(CustomResultType<>).Name}' is not supported."
            );
    }

    [Fact]
    public async Task
        GivenOkResultWithEmptyValue_WhenInvokingMatchAsync_ThenInvokeOkHandler()
    {
        // Arrange
        var okHandler = Substitute.For<
            Func<CancellationToken, Task<ITestType>>>();

        var errorHandler = Substitute
           .For<Func<Error, CancellationToken, Task<ITestType>>>();

        Task<ResultType<Empty>> ok = Result.OkAsync();

        // Act
        ITestType result = await ok.MatchAsync(
            okHandler,
            errorHandler,
            CancellationToken.None
        );

        // Assert
        result.Should().NotBeNull();
        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task
        GivenErrorResultWithEmptyValue_WhenInvokingMatchAsync_ThenInvokeErrorHandler()
    {
        // Arrange
        var okHandler = Substitute.For<
            Func<CancellationToken, Task<ITestType>>>();

        var errorHandler = Substitute
           .For<Func<Error, CancellationToken, Task<ITestType>>>();

        var error = _fixture.Create<Error>();

        Task<ResultType<Empty>> ok = Result.ErrorAsync(error);

        // Act
        ITestType result = await ok.MatchAsync(
            okHandler,
            errorHandler,
            CancellationToken.None
        );

        // Assert
        result.Should().NotBeNull();
        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }

    [Fact]
    public async Task
        GivenOkResultWithValue_AndVoidHandler_WhenInvokingMatchAsync_ThenInvokeOkHandler()
    {
        // Arrange
        var okHandler =
            Substitute.For<Func<ITestType, CancellationToken, Task>>();
        var errorHandler = Substitute.For<
            Func<Error, CancellationToken, Task>>();

        Task<ResultType<ITestType>> result =
            Result.OkAsync(Substitute.For<ITestType>());

        // Act
        await result.MatchAsync(
            okHandler,
            errorHandler,
            CancellationToken.None
        );

        // Assert
        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task
        GivenErrorResultWithValue_AndVoidHandler_WhenInvokingMatchAsync_ThenInvokeErrorHandler()
    {
        // Arrange
        var okHandler =
            Substitute.For<Func<ITestType, CancellationToken, Task>>();
        var errorHandler = Substitute.For<
            Func<Error, CancellationToken, Task>>();

        var error = _fixture.Create<Error>();

        Task<ResultType<ITestType>>
            result = Result.ErrorAsync<ITestType>(error);

        // Act
        await result.MatchAsync(
            okHandler,
            errorHandler,
            CancellationToken.None
        );

        // Assert
        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }

    [Fact]
    public async Task
        GivenOkResultWithEmptyValue_AndVoidHandler_WhenInvokingMatchAsync_ThenInvokeOkHandler()
    {
        // Arrange
        var okHandler =
            Substitute.For<Func<CancellationToken, Task>>();
        var errorHandler = Substitute.For<
            Func<Error, CancellationToken, Task>>();

        Task<ResultType<Empty>> result =
            Result.OkAsync();

        // Act
        await result.MatchAsync(
            okHandler,
            errorHandler,
            CancellationToken.None
        );

        // Assert
        okHandler.ReceivedCalls().Should().ContainSingle();
        errorHandler.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task
        GivenErrorResultWithEmptyValue_AndVoidHandler_WhenInvokingMatchAsync_ThenInvokeErrorHandler()
    {
        // Arrange
        var okHandler =
            Substitute.For<Func<CancellationToken, Task>>();
        var errorHandler = Substitute.For<
            Func<Error, CancellationToken, Task>>();

        var error = _fixture.Create<Error>();

        Task<ResultType<Empty>>
            result = Result.ErrorAsync(error);

        // Act
        await result.MatchAsync(
            okHandler,
            errorHandler,
            CancellationToken.None
        );

        // Assert
        okHandler.ReceivedCalls().Should().BeEmpty();
        errorHandler.ReceivedCalls().Should().ContainSingle();
    }
}
