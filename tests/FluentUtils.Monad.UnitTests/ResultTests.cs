namespace FluentUtils.Monad.UnitTests;

using AutoFixture;
using FluentAssertions;
using Monad.Extensions;
using NSubstitute;

public class ResultTests
{
    private readonly Fixture _fixture;

    public ResultTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void WhenInvokingNonGenericOk_ThenReturnOkResultOfEmpty()
    {
        ResultType<Empty> result = Result.Ok();

        result.Should().BeOfType<OkResultType<Empty>>();
    }

    [Fact]
    public void WhenInvokingOkWithType_ThenReturnOkResultOfTheSameType()
    {
        var testType = Substitute.For<ITestType>();
        ResultType<ITestType> result = Result.Ok(testType);

        result.Should().BeOfType<OkResultType<ITestType>>();
        result.Unwrap().Should().BeSameAs(testType);
    }

    [Fact]
    public void WhenInvokingNonGenericError_ThenReturnErrorResultOfEmpty()
    {
        var error = _fixture.Create<Error>();
        ResultType<Empty> result = Result.Error(error);

        result.Should().BeOfType<ErrorResultType<Empty>>();
    }

    [Fact]
    public void WhenInvokingErrorWithType_THenReturnErrorResultOfTheSameType()
    {
        var error = _fixture.Create<Error>();
        ResultType<ITestType> result = Result.Error<ITestType>(error);

        result.Should().BeOfType<ErrorResultType<ITestType>>();
    }

    [Fact]
    public void
        WhenInvokingErrorWithMessage_ThenReturnErrorWithAutomaticallyGeneratedCode()
    {
        ResultType<Empty> result =
            Result.Error("Testing automatic error code creation");

        Error error = result.Match(_ => default!, error => error);

        string[] errorCodeParts = error.Code.Value.Split('_');
        errorCodeParts.Length.Should().Be(2);
        errorCodeParts[0].Should().Be("WIE");

        bool isNumber = int.TryParse(errorCodeParts[1], out int number);
        isNumber.Should().BeTrue();
        number.Should().BeGreaterThan(0);
    }
}
