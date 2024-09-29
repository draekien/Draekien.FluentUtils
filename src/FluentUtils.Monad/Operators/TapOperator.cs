namespace FluentUtils.Monad.Operators;

[PublicAPI]
public static class TapOperator
{
    [DebuggerStepperBoundary]
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

    [DebuggerStepperBoundary]
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

    public static async Task<ResultType<TIn>> Tap<TIn>(
        this Task<ResultType<TIn>> resultTask,
        Action<TIn> tap) =>
        (await resultTask).Tap(tap);

    public static async Task<ResultType<TIn>> Tap<TIn>(
        this Task<ResultType<Task<TIn>>> resultTask,
        Action<TIn> tap)
    {
        ResultType<Task<TIn>> result = await resultTask;
        return await result.Match(
            async valueTask =>
            {
                TIn value = await valueTask;
                tap(value);
                return await Result.OkAsync(value);
            },
            Result.ErrorAsync<TIn>);
    }

    public static async Task<ResultType<TIn>> Tap<TIn>(
        this Task<ResultType<Task<TIn>>> resultTask,
        Func<TIn, Task> tap)
    {
        ResultType<Task<TIn>> result = await resultTask;
        return await result.Match(
            async valueTask =>
            {
                TIn value = await valueTask;
                await tap(value);
                return await Result.OkAsync(value);
            },
            Result.ErrorAsync<TIn>);
    }
}
