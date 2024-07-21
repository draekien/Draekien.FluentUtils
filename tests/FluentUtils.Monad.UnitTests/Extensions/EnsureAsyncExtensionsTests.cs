namespace FluentUtils.Monad.UnitTests.Extensions;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Monad.Extensions;

public class EnsureAsyncExtensionsTests
{
    private readonly Fixture _fixture;

    public EnsureAsyncExtensionsTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Fact]
    public async Task
        GivenResultValueMatchesPredicate_WhenEnsuringResult_ThenReturnOkResult()
    {
        // Arrange
        var testType = _fixture.Create<ITestType>();
        Task<ResultType<ITestType>> result = Result.OkAsync(testType);

        // Act
        ResultType<ITestType> output =
            await result.EnsureAsync((_, _) => Task.FromResult(true));

        // Assert
        output.Should().BeOfType<OkResultType<ITestType>>();
    }

    [Fact]
    public async Task
        GivenResultValueDoesNotMatchPredicate_WhenEnsuringResult_ThenReturnErrorResult()
    {
        // Arrange
        var testType = _fixture.Create<ITestType>();
        Task<ResultType<ITestType>> result = Result.OkAsync(testType);

        // Act
        ResultType<ITestType> output =
            await result.EnsureAsync((_, _) => Task.FromResult(false));

        // Assert
        output.Should().BeOfType<ErrorResultType<ITestType>>();
        output.As<ErrorResultType<ITestType>>()
           .Error.Should()
           .Be(MonadErrors.FailedPredicate);
    }

    [Fact]
    public async Task
        GivenResultValueDoesNotMatchPredicate_AndCustomErrorIsProvided_WhenEnsuringResult_ThenReturnCustomError()
    {
        // Arrange
        var testType = _fixture.Create<ITestType>();
        Task<ResultType<ITestType>> result = Result.OkAsync(testType);
        var customError = _fixture.Freeze<Error>();

        // Act
        ResultType<ITestType> output = await result.EnsureAsync(
            (_, _) => Task.FromResult(false),
            customError);

        // Assert
        output.Should().BeOfType<ErrorResultType<ITestType>>();
        output.As<ErrorResultType<ITestType>>()
           .Error.Should()
           .Be(customError);
    }

    [Fact]
    public async Task
        GivenResultIsAlreadyErrored_WhenEnsuringResult_ThenForwardError()
    {
        // Arrange
        var customError = _fixture.Freeze<Error>();
        Task<ResultType<ITestType>> errorResult =
            Result.ErrorAsync<ITestType>(customError);

        // Act
        ResultType<ITestType> output =
            await errorResult.EnsureAsync((_, _) => Task.FromResult(true));

        // Assert
        output.Should().BeOfType<ErrorResultType<ITestType>>();
        output.As<ErrorResultType<ITestType>>()
           .Error.Should()
           .Be(customError);
    }
}
