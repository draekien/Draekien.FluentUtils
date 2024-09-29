namespace FluentUtils.Monad.Operators;

[PublicAPI]
public static class EnsureOperator
{
    [DebuggerStepperBoundary]
    public static ResultType<TIn> Ensure<TIn>(
        this ResultType<TIn> result,
        Func<TIn, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = "")
    {
        return result.Match(
            value =>
            {
                try
                {
                    result.Logger.LogDebug(
                        "Invoking predicate on value of type {ValueType}",
                        result.ValueType.Name);

                    if (!predicate.Invoke(value))
                    {
                        throw new ArgumentException(
                            "Value did not satisfy predicate",
                            nameof(value));
                    }

                    result.Logger.LogDebug(
                        "Value of type {ValueType} satisfies predicate",
                        result.ValueType.Name);

                    return value;
                }
                catch (Exception ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "Value of type {ValueType} does not satisfy predicate",
                        result.ValueType.Name);

                    return MonadErrors.FailedPredicate(predicateExpression, ex);
                }
            },
            Result.Error<TIn>);
    }

    [DebuggerStepperBoundary]
    public static async Task<ResultType<TIn>> Ensure<TIn>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, Task<bool>> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = "")
    {
        ResultType<TIn> result = await resultTask;

        return await result.Match(
            async value =>
            {
                try
                {
                    result.Logger.LogDebug(
                        "Invoking predicate on value of type {ValueType}",
                        result.ValueType.Name);

                    if (!await predicate(value))
                    {
                        throw new ArgumentException(
                            "Value did not satisfy predicate",
                            nameof(value));
                    }

                    result.Logger.LogDebug(
                        "Value of type {ValueType} satisfies predicate",
                        result.ValueType.Name);

                    return value;
                }
                catch (Exception ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "Value of type {ValueType} does not satisfy predicate",
                        result.ValueType.Name);

                    return MonadErrors.FailedPredicate(predicateExpression, ex);
                }
            },
            Result.ErrorAsync<TIn>);
    }
}
