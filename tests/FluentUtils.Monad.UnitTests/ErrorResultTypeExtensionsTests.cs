namespace FluentUtils.Monad.UnitTests;

using FluentAssertions;

public class ErrorResultTypeExtensionsTests
{
    [Fact]
    public void
        GivenErrorResult_WhenInvokingTo_ThenConvertErrorToDifferentType()
    {
        ResultType<ITestType> errorResult = Result.Error<ITestType>("Test");
        ResultType<Empty> result =
            (errorResult as ErrorResultType<ITestType>)!.To<Empty>();

        result.Should().BeOfType<ErrorResultType<Empty>>();
    }
}
