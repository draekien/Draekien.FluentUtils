namespace FluentUtils.Monad.Operators;

/// <summary>
///     Ensures the value of a result satisfies a predicate
/// </summary>
[PublicAPI]
public static class EnsureOperator
{
    /// <summary>
    ///     Ensures the value of a <see cref="ResultType{T}" /> satisfies a predicate
    /// </summary>
    /// <remarks>
    ///     The predicate will only be executed if the input
    ///     <see cref="ResultType{T}" />
    ///     is an <see cref="OkResultType{T}" />
    /// </remarks>
    /// <param name="result">The <see cref="ResultType{T}" /></param>
    /// <param name="predicate">The condition the value must satisfy</param>
    /// <param name="predicateExpression">The predicate expression string</param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <returns>
    ///     An <see cref="OkResultType{T}" /> if the predicate is satisfied,
    ///     otherwise an <see cref="ErrorResultType{T}" />
    /// </returns>
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

                    return result;
                }
                catch (Exception ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "Value of type {ValueType} does not satisfy predicate",
                        result.ValueType.Name);

                    return Result.Error<TIn>(
                        MonadErrors.FailedPredicate(predicateExpression, ex),
                        result.Logger);
                }
            },
            error => Result.Error<TIn>(error, result.Logger));
    }

    /// <summary>
    ///     Ensures the value of a <see cref="ResultType{T}" /> satisfies a predicate
    /// </summary>
    /// <remarks>
    ///     The predicate will only be executed if the input
    ///     <see cref="ResultType{T}" />
    ///     is an <see cref="OkResultType{T}" />
    /// </remarks>
    /// <param name="resultTask">The <see cref="ResultType{T}" /></param>
    /// <param name="predicate">The condition the value must satisfy</param>
    /// <param name="predicateExpression">The predicate expression string</param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <returns>
    ///     An <see cref="OkResultType{T}" /> if the predicate is satisfied,
    ///     otherwise an <see cref="ErrorResultType{T}" />
    /// </returns>
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

                    return result;
                }
                catch (Exception ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "Value of type {ValueType} does not satisfy predicate",
                        result.ValueType.Name);

                    return await Result.ErrorAsync<TIn>(
                        MonadErrors.FailedPredicate(predicateExpression, ex),
                        result.Logger);
                }
            },
            error => Result.ErrorAsync<TIn>(error, result.Logger));
    }
}
