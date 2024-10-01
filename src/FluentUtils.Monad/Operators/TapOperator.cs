namespace FluentUtils.Monad.Operators;

/// <summary>
///     Access the value of a result
/// </summary>
[PublicAPI]
public static class TapOperator
{
    /// <summary>
    ///     Tap into the value of a <see cref="ResultType{T}" /> to perform a
    ///     side effect
    /// </summary>
    /// <remarks>
    ///     The tap operator will only be executed if the input
    ///     <see cref="ResultType{T}" /> is an <see cref="OkResultType{T}" />
    /// </remarks>
    /// <param name="result">The <see cref="ResultType{T}" /></param>
    /// <param name="tap">The side effect</param>
    /// <param name="tapExpression">The side effect expression</param>
    /// <typeparam name="TIn">The result value's type</typeparam>
    /// <returns>The original result</returns>
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

                    return result;
                }
                catch (Exception ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "An exception occured while invoking the side effect");
                    return Result.Error<TIn>(
                        MonadErrors.FailedToTapValue(ex, tapExpression),
                        result.Logger);
                }
            },
            error => Result.Error<TIn>(error, result.Logger));
    }

    /// <summary>
    ///     Tap into the value of a <see cref="ResultType{T}" /> to perform a
    ///     side effect
    /// </summary>
    /// <remarks>
    ///     The tap operator will only be executed if the input
    ///     <see cref="ResultType{T}" /> is an <see cref="OkResultType{T}" />
    /// </remarks>
    /// <param name="resultTask">The <see cref="ResultType{T}" /></param>
    /// <param name="tap">The side effect</param>
    /// <param name="tapExpression">The side effect expression</param>
    /// <typeparam name="TIn">The result value's type</typeparam>
    /// <returns>The original result</returns>
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

                    return result;
                }
                catch (Exception ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "An exception occured while invoking the side effect");
                    return await Result.ErrorAsync<TIn>(
                        MonadErrors.FailedToTapValue(ex, tapExpression),
                        result.Logger);
                }
            },
            error => Result.ErrorAsync<TIn>(error, result.Logger));
    }

    /// <summary>
    ///     Tap into the value of a <see cref="ResultType{T}" /> to perform a
    ///     side effect
    /// </summary>
    /// <remarks>
    ///     The tap operator will only be executed if the input
    ///     <see cref="ResultType{T}" /> is an <see cref="OkResultType{T}" />
    /// </remarks>
    /// <param name="resultTask">The <see cref="ResultType{T}" /></param>
    /// <param name="tap">The side effect</param>
    /// <typeparam name="TIn">The result value's type</typeparam>
    /// <returns>The original result</returns>
    [DebuggerStepperBoundary]
    public static async Task<ResultType<TIn>> Tap<TIn>(
        this Task<ResultType<TIn>> resultTask,
        Action<TIn> tap) =>
        (await resultTask).Tap(tap);
}
