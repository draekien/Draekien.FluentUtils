namespace FluentUtils.Monad.UnitTests.Extensions;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Monad.Extensions;

public class EnsureExtensionsTests
{
    private readonly Fixture _fixture;

    public EnsureExtensionsTests()
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
           .Error.Should()
           .Be(MonadErrors.FailedPredicate("_ => false"));
    }

    [Fact]
    public void
        GivenResultValueDoesNotMatchPredicate_AndCustomErrorIsProvided_WhenEnsuringResult_ThenReturnCustomError()
    {
        // Arrange
        var testType = _fixture.Create<ITestType>();
        ResultType<ITestType> result = Result.Ok(testType);
        var customError = _fixture.Freeze<Error>();

        // Act
        ResultType<ITestType> output = result.Ensure(_ => false, customError);

        // Assert
        output.Should().BeOfType<ErrorResultType<ITestType>>();
        output.As<ErrorResultType<ITestType>>()
           .Error.Should()
           .Be(customError);
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
}
