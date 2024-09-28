namespace FluentUtils.Monad.UnitTests.Operators;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Monad.Operators;

public class EnsureOperatorTests
{
    private readonly Fixture _fixture;

    public EnsureOperatorTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Fact]
    public void
        GivenResultValueMatchesPredicate_WhenEnsuringResult_ThenReturnOkResult()
    {
        // Arrange
        var testType = _fixture.Create<ITestType>();
        ResultType<ITestType> result = Result.Ok(testType);

        // Act
        ResultType<ITestType> output = result.Ensure(_ => true);

        // Assert
        output.Should().BeOfType<OkResultType<ITestType>>();
    }

    [Fact]
    public void
        GivenResultValueDoesNotMatchPredicate_WhenEnsuringResult_ThenReturnErrorResult()
    {
        // Arrange
        var testType = _fixture.Create<ITestType>();
        ResultType<ITestType> result = Result.Ok(testType);

        // Act
        ResultType<ITestType> output = result.Ensure(_ => false);

        // Assert
        output.Should().BeOfType<ErrorResultType<ITestType>>();
        output.As<ErrorResultType<ITestType>>()
              .Error.Message.Value.Should()
              .Be("Result value does not match predicate '_ => false'.");
    }

    [Fact]
    public void
        GivenResultIsAlreadyErrored_WhenEnsuringResult_ThenForwardError()
    {
        // Arrange
        var customError = _fixture.Freeze<Error>();
        ResultType<ITestType>
            errorResult = Result.Error<ITestType>(customError);

        // Act
        ResultType<ITestType> output = errorResult.Ensure(_ => true);

        // Assert
        output.Should().BeOfType<ErrorResultType<ITestType>>();
        output.As<ErrorResultType<ITestType>>()
              .Error.Should()
              .Be(customError);
    }

    [Fact]
    public async Task
        GivenResultValueMatchesPredicate_WhenEnsuringAsyncResult_ThenReturnOkResult()
    {
        // Arrange
        var testType = _fixture.Create<ITestType>();
        Task<ResultType<ITestType>> result = Result.OkAsync(testType);

        // Act
        ResultType<ITestType> output =
            await result.Ensure(_ => Task.FromResult(true));

        // Assert
        output.Should().BeOfType<OkResultType<ITestType>>();
    }

    [Fact]
    public async Task
        GivenResultValueDoesNotMatchPredicate_WhenEnsuringAsyncResult_ThenReturnErrorResult()
    {
        // Arrange
        var testType = _fixture.Create<ITestType>();
        Task<ResultType<ITestType>> result = Result.OkAsync(testType);

        // Act
        ResultType<ITestType> output =
            await result.Ensure(_ => Task.FromResult(false));

        // Assert
        output.Should().BeOfType<ErrorResultType<ITestType>>();
        output.As<ErrorResultType<ITestType>>()
              .Error.Message.Value.Should()
              .Be(
                   "Result value does not match predicate '_ => Task.FromResult(false)'.");
    }

    [Fact]
    public async Task
        GivenResultIsAlreadyErrored_WhenEnsuringAsyncResult_ThenForwardError()
    {
        // Arrange
        var customError = _fixture.Freeze<Error>();
        Task<ResultType<ITestType>> errorResult =
            Result.ErrorAsync<ITestType>(customError);

        // Act
        ResultType<ITestType> output =
            await errorResult.Ensure(_ => Task.FromResult(true));

        // Assert
        output.Should().BeOfType<ErrorResultType<ITestType>>();
        output.As<ErrorResultType<ITestType>>()
              .Error.Should()
              .Be(customError);
    }
}
