namespace FluentUtils.Monad.UnitTests.Operators;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Exceptions;
using FluentAssertions;
using Monad.Operators;

public class UnwrapOperatorTests
{
    private readonly Fixture _fixture;

    public UnwrapOperatorTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Fact]
    public async Task GivenOkResult_WhenInvokingUnwrapAsync_ThenReturnValue()
    {
        // Arrange
        var value = _fixture.Freeze<ITestType>();

        Task<ResultType<ITestType>> okResult = Result.OkAsync(value);

        // Act
        ITestType result = await okResult.Unwrap();

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    public async Task
        GivenErrorResult_WhenInvokingUnwrapAsync_ThenThrowUnwrapPanicException()
    {
        // Arrange
        var error = _fixture.Freeze<Error>();

        Task<ResultType<ITestType>> okResult =
            Result.ErrorAsync<ITestType>(error);

        // Act + Assert
        await okResult.Invoking(x => x.Unwrap())
                      .Should()
                      .ThrowAsync<UnwrapPanicException>()
                      .WithMessage(error.ToString());
    }
}
