namespace FluentUtils.Monad.Operators;

using System.Diagnostics;
using Microsoft.Extensions.Logging;

[PublicAPI]
public static class UnwrapOperator
{
    [DebuggerStepperBoundary]
    public static T Unwrap<T>(this ResultType<T> result) =>
        result.Match(
            value => value,
            error =>
            {
                UnwrapPanicException exception = new(error);

                result.Logger.LogError(
                    exception,
                    "Failed to unwrap an ErrorResult of {ValueType}",
                    result.ValueType.Name);

                throw exception;
            });

    [DebuggerStepperBoundary]
    public static async Task<T> Unwrap<T>(
        this Task<ResultType<T>> resultTask) =>
        (await resultTask).Unwrap();
}
