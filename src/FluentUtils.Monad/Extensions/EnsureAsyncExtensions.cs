namespace FluentUtils.Monad.Extensions;

/// <summary>
///     Extensions for ensuring the value of an asynchronous
///     <see cref="ResultType{T}" />
/// </summary>
[PublicAPI]
public static class EnsureAsyncExtensions
{
    /// <summary>
    ///     Ensures the value inside an asynchronous <see cref="ResultType{T}" />
    ///     matches the provided predicate
    /// </summary>
    /// <param name="resultTask">
    ///     A task which when completed returns a
    ///     <see cref="ResultType{T}" />
    /// </param>
    /// <param name="predicate">An asynchronous predicate to execute</param>
    /// <param name="error">
    ///     Optional. The <see cref="Error" /> that will be assigned to
    ///     the result if it does not pass the predicate
    /// </param>
    /// <typeparam name="T">The result value type</typeparam>
    /// <returns>The original result on success, otherwise an error result</returns>
    public static Task<ResultType<T>> EnsureAsync<T>(
        this Task<ResultType<T>> resultTask,
        Func<T, CancellationToken, Task<bool>> predicate,
        Error? error = default) where T : notnull => resultTask.MatchAsync(
        async (value, ct) => await predicate(value, ct)
            ? await resultTask
            : await Result.ErrorAsync<T>(MonadErrors.FailedPredicate, ct),
        Result.ErrorAsync<T>);
}
