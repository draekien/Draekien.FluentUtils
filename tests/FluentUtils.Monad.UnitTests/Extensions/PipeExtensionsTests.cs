namespace FluentUtils.Monad.UnitTests.Extensions;

using AutoFixture;
using FluentAssertions;
using Monad.Extensions;
using NSubstitute;

public class PipeExtensionsTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void
        GivenOkResult_WhenInvokingPipe_ThenMapValueToNewType()
    {
        // Arrange
        ResultType<Empty> ok = Result.Ok();
        var expected = Substitute.For<ITestType>();

        // Act
        ResultType<ITestType> result =
            ok.Pipe(_ => expected);

        // Assert
        result.Should().BeOfType<OkResultType<ITestType>>();
        result.Unwrap().Should().Be(expected);
    }

    [Fact]
    public void
        GivenErrorResult_WhenInvokingPipe_ThenMapErrorToNewType()
    {
        // Arrange
        var error = _fixture.Create<Error>();
        ResultType<Empty> errorResult = Result.Error(error);
        ResultType<ITestType> expected =
            Result.Error<ITestType>(error);

        // Act
        ResultType<ITestType> result = errorResult.Pipe(
            _ => Substitute.For<ITestType>()
        );

        // Assert
        result.Should().BeOfType<ErrorResultType<ITestType>>();
        result.As<ErrorResultType<ITestType>>()
           .Should()
           .Be(expected);
    }

    [Fact]
    public void
        GivenExceptionFromPipeExpression_WhenInvokingPipe_ThenReturnErrorWithPipeExpression()
    {
        // Arrange
        ResultType<string> sut = Result.Bind(() => "bob");

        // Act
        ResultType<bool> result = sut.Pipe(
            _ =>
            {
                throw new InvalidOperationException();

#pragma warning disable CS0162 // Unreachable code detected - required for test
                return false;
#pragma warning restore CS0162 // Unreachable code detected
            });

        // Assert
        result.Should().BeOfType<ErrorResultType<bool>>();
        result.As<ErrorResultType<bool>>()
           .Error.Message.Value.Should()
           .Contain("throw new InvalidOperationException()");
    }
}
