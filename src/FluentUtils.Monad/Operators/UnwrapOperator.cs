namespace FluentUtils.Monad.Operators;

/// <summary>
///     Operators for unwrapping the value of a <see cref="ResultType{T}" />
/// </summary>
[PublicAPI]
public static class UnwrapOperator
{
    /// <summary>
    ///     Unwrap the value of a <see cref="ResultType{T}" />
    ///     without checking to see if it is an instance of
    ///     <see cref="OkResultType{T}" />
    /// </summary>
    /// <param name="result">The <see cref="ResultType{T}" /> to be unwrapped</param>
    /// <typeparam name="T">The result value's type</typeparam>
    /// <returns>
    ///     The value stored inside the result when it is an
    ///     <see cref="OkResultType{T}" />
    /// </returns>
    /// <exception cref="UnwrapPanicException">
    ///     The result is an <see cref="ErrorResultType{T}" />
    /// </exception>
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

    /// <summary>
    ///     Unwrap the value of a <see cref="ResultType{T}" />
    ///     without checking to see if it is an instance of
    ///     <see cref="OkResultType{T}" />
    /// </summary>
    /// <param name="resultTask">The <see cref="ResultType{T}" /> to be unwrapped</param>
    /// <typeparam name="T">The result value's type</typeparam>
    /// <returns>
    ///     The value stored inside the result when it is an
    ///     <see cref="OkResultType{T}" />
    /// </returns>
    /// <exception cref="UnwrapPanicException">
    ///     The result is an <see cref="ErrorResultType{T}" />
    /// </exception>
    [DebuggerStepperBoundary]
    public static async Task<T> Unwrap<T>(
        this Task<ResultType<T>> resultTask) =>
        (await resultTask).Unwrap();
}
