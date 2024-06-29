namespace FluentUtils.Monad.UnitTests.Extensions;

using AutoFixture;
using FluentAssertions;
using Monad.Extensions;
using NSubstitute;

public class MapExtensionsTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void
        GivenOkResult_WhenInvokingMap_ThenMapValueToNewType()
    {
        // Arrange
        ResultType<Empty> ok = Result.Ok();
        var expected = Substitute.For<ITestType>();

        // Act
        ResultType<ITestType> result =
            ok.Map(_ => Result.Ok(expected));

        // Assert
        result.Should().BeOfType<OkResultType<ITestType>>();
        result.Unwrap().Should().Be(expected);
    }

    [Fact]
    public void
        GivenErrorResult_WhenInvokingMap_ThenMapErrorToNewType()
    {
        // Arrange
        var error = _fixture.Create<Error>();
        ResultType<Empty> errorResult = Result.Error(error);
        ResultType<ITestType> expected =
            Result.Error<ITestType>(error);

        // Act
        ResultType<ITestType> result = errorResult.Map(
            _ => Result.Ok(Substitute.For<ITestType>())
        );

        // Assert
        result.Should().BeOfType<ErrorResultType<ITestType>>();
        result.As<ErrorResultType<ITestType>>()
           .Should()
           .Be(expected);
    }
}
