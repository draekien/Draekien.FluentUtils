namespace FluentUtils.Monad.Operators;

using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

[PublicAPI]
public static class TapOperator
{
    public static ResultType<TIn> Tap<TIn>(
        this ResultType<TIn> result,
        Action<TIn> tap,
        [CallerArgumentExpression(nameof(tap))]
        string tapExpression = "")
    {
        return result.Match(
            value =>
            {
                try
                {
                    result.Logger.LogDebug(
                        "Invoking side effect on value of type {ValueType}",
                        result.ValueType.Name);

                    tap.Invoke(value);

                    result.Logger.LogDebug(
                        "Invoking side effect completed successfully");

                    return Result.Ok(value);
                }
                catch (Exception ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "An exception occured while invoking the side effect");
                    return MonadErrors.FailedToTapValue(ex, tapExpression);
                }
            },
            Result.Error<TIn>);
    }

    public static async Task<ResultType<TIn>> Tap<TIn>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, Task> tap,
        [CallerArgumentExpression(nameof(tap))]
        string tapExpression = "")
    {
        ResultType<TIn> result = await resultTask;

        return await result.Match(
            async value =>
            {
                try
                {
                    result.Logger.LogDebug(
                        "Invoking side effect on value of type {ValueType}",
                        result.ValueType.Name);

                    await tap(value);

                    result.Logger.LogDebug(
                        "Invoking side effect completed successfully");

                    return value;
                }
                catch (Exception ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "An exception occured while invoking the side effect");
                    return MonadErrors.FailedToTapValue(ex, tapExpression);
                }
            },
            Result.ErrorAsync<TIn>);
    }
}
