namespace FluentUtils.Monad.Extensions;

/// <summary>
///     Extensions for ensuring a <see cref="ResultType{T}" /> satisfies a
///     predicate
/// </summary>
[PublicAPI]
public static class EnsureExtensions
{
    /// <summary>
    ///     Ensures the value inside a <see cref="ResultType{T}" /> matches the
    ///     provided predicate
    /// </summary>
    /// <param name="result">The <see cref="ResultType{T}" /> to be checked</param>
    /// <param name="predicate">
    ///     The predicate that will be used to evaluate the
    ///     <see cref="ResultType{T}" />
    /// </param>
    /// <param name="error">
    ///     Optional. The <see cref="Error" /> that will be assigned to
    ///     the result if it does not pass the predicate
    /// </param>
    /// <typeparam name="T">The result value type</typeparam>
    /// <returns>The original result on success, otherwise an error result</returns>
    public static ResultType<T> Ensure<T>(
        this ResultType<T> result,
        Func<T, bool> predicate,
        Error? error = default) where T : notnull =>
        result.Match(
            value => predicate(value)
                ? result
                : Result.Error<T>(error ?? MonadErrors.FailedPredicate),
            Result.Error<T>);
}
