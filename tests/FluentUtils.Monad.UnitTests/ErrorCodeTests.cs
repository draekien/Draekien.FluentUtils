namespace FluentUtils.Monad.UnitTests;

using FluentAssertions;

public class ErrorCodeTests
{
    [Fact]
    public void
        GivenString_WhenImplicitlyConvertingToErrorCode_ThenReturnExpectedErrorCode()
    {
        // Arrange
        const string test = nameof(test);

        // Act
        ErrorCode code = test;

        // Assert
        code.Should().Be(new ErrorCode(nameof(test)));
    }
}
