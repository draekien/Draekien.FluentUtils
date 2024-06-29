namespace FluentUtils.Monad.UnitTests;

using AutoFixture;
using FluentAssertions;

public class ResultTypeTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void
        GivenException_WhenImplicitlyConvertingToResultType_ThenReturnErrorResultType()
    {
        // Arrange
        var exception =
            _fixture.Create<InvalidOperationException>();

        // Act
        ResultType<Empty> result = exception;

        // Assert
        result.Should().BeOfType<ErrorResultType<Empty>>();
        result.As<ErrorResultType<Empty>>()
           .Error.Code.Value.Should()
           .Be("DYN_IOE");

        result.As<ErrorResultType<Empty>>()
           .Error.Message.Value.Should()
           .Be(exception.Message);

        result.As<ErrorResultType<Empty>>()
           .Error.Exception.Should()
           .Be(exception);
    }
}
