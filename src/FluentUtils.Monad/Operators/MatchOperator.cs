namespace FluentUtils.Monad.Operators;

[PublicAPI]
public static class MatchOperator
{
    [DebuggerStepperBoundary]
    public static TOut Match<TIn, TOut>(
        this ResultType<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Error, TOut> onError)
    {
        switch (result)
        {
            case OkResultType<TIn> okResult:
                result.Logger.LogDebug(
                    "Matched on an OK result for {ValueType}, invoking onSuccess",
                    result.ValueType.Name);

                TOut successOutput = onSuccess.Invoke(okResult.Value);

                result.Logger.LogDebug(
                    "Invoking onSuccess completed successfully");

                return successOutput;
            case ErrorResultType<TIn> errorResult:
                result.Logger.LogDebug(
                    "Matched on an Error result for {ValueType}, invoking onError",
                    result.ValueType.Name);

                TOut errorOutput = onError.Invoke(errorResult.Error);

                result.Logger.LogDebug(
                    "Invoking onError completed successfully");

                return errorOutput;
            default:
                UnsupportedResultTypeException<TIn> exception = new(result);
                result.Logger.LogError(
                    exception,
                    "A custom 'ResultType<T>' of type '{ResultType}' is not supported",
                    result.GetType().Name);
                throw exception;
        }
    }

    [DebuggerStepperBoundary]
    public static async Task<TOut> Match<TIn, TOut>(
        this ResultType<TIn> result,
        Func<TIn, Task<TOut>> onSuccess,
        Func<Error, Task<TOut>> onError)
    {
        switch (result)
        {
            case OkResultType<TIn> okResult:
                result.Logger.LogDebug(
                    "Matched on an OK result for {ValueType}, invoking onSuccess",
                    result.ValueType.Name);

                TOut successOutput = await onSuccess.Invoke(okResult.Value);

                result.Logger.LogDebug(
                    "Invoking onSuccess completed successfully");

                return successOutput;
            case ErrorResultType<TIn> errorResult:
                result.Logger.LogDebug(
                    "Matched on an Error result for {ValueType}, invoking onError",
                    result.ValueType.Name);

                TOut errorOutput = await onError.Invoke(errorResult.Error);

                result.Logger.LogDebug(
                    "Invoking onError completed successfully");

                return errorOutput;
            default:
                UnsupportedResultTypeException<TIn> exception = new(result);
                result.Logger.LogError(
                    exception,
                    "A custom 'ResultType<T>' of type '{ResultType}' is not supported",
                    result.GetType().Name);
                throw exception;
        }
    }

    [DebuggerStepperBoundary]
    public static async Task<TOut> Match<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, TOut> onSuccess,
        Func<Error, TOut> onError)
    {
        ResultType<TIn> result = await resultTask;
        return result.Match(onSuccess, onError);
    }

    [DebuggerStepperBoundary]
    public static async Task<TOut> Match<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, Task<TOut>> onSuccess,
        Func<Error, Task<TOut>> onError)
    {
        ResultType<TIn> result = await resultTask;
        return await result.Match(onSuccess, onError);
    }

    [DebuggerStepperBoundary]
    public static void Match<TIn>(
        this ResultType<TIn> result,
        Action<TIn> onSuccess,
        Action<Error> onError)
    {
        result.Match(
            value =>
            {
                onSuccess(value);
                return Empty.Default;
            },
            error =>
            {
                onError(error);
                return Empty.Default;
            });
    }
}
