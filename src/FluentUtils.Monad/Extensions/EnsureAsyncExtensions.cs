namespace FluentUtils.Monad.Extensions;

using System.Diagnostics;
using System.Runtime.CompilerServices;

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
    /// <param name="predicateExpression">The predicate expression</param>
    /// <typeparam name="T">The result value type</typeparam>
    /// <returns>The original result on success, otherwise an error result</returns>
    [DebuggerStepperBoundary]
    public static Task<ResultType<T>> EnsureAsync<T>(
        this Task<ResultType<T>> resultTask,
        Func<T, Task<bool>> predicate,
        Error? error = default,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = "not provided") => resultTask.MatchAsync(
        async value =>
        {
            bool passed = await predicate(value);

            if (passed) return await resultTask;

            return error ?? MonadErrors.FailedPredicate(predicateExpression);
        },
        Result.ErrorAsync<T>);
}
