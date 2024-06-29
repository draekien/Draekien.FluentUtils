namespace FluentUtils.Monad.UnitTests.Extensions;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Exceptions;
using FluentAssertions;
using Monad.Extensions;

public class UnwrapAsyncTests
{
    private readonly Fixture _fixture;

    public UnwrapAsyncTests()
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
        ITestType result = await okResult.UnwrapAsync();

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    public async Task
        GivenErroResult_WhenInvokingUnwrapAsync_ThenThrowUnwrapPanicException()
    {
        // Arrange
        var error = _fixture.Freeze<Error>();

        Task<ResultType<ITestType>> okResult =
            Result.ErrorAsync<ITestType>(error);

        // Act + Assert
        await okResult.Invoking(x => x.UnwrapAsync())
           .Should()
           .ThrowAsync<UnwrapPanicException>()
           .WithMessage(error.ToString());
    }
}
