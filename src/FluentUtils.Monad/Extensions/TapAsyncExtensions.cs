namespace FluentUtils.Monad.Extensions;

using System.Diagnostics;
using System.Runtime.CompilerServices;

/// <summary>
///     Asynchronous extension methods for tapping into the value of a
///     <see cref="ResultType{T}" />
/// </summary>
public static class TapAsyncExtensions
{
    /// <summary>
    ///     Tap into the value of an asynchronous <see cref="ResultType{T}" /> to
    ///     invoke a side effect.
    /// </summary>
    /// <param name="resultTask">The <see cref="ResultType{T}" /></param>
    /// <param name="tapAsync">The side effect</param>
    /// <param name="tapExpression">The side effect expression string</param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <returns>The input result</returns>
    [DebuggerStepperBoundary]
    public static Task<ResultType<TIn>> TapAsync<TIn>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, Task> tapAsync,
        [CallerArgumentExpression(nameof(tapAsync))]
        string tapExpression =
            "") => resultTask.PipeAsync(
        async value =>
        {
            try
            {
                await tapAsync(value);
                return await resultTask;
            }
            catch (Exception ex)
            {
                return await Result.ErrorAsync<TIn>(
                    MonadErrors.FailedToTapValue(ex, tapExpression));
            }
        });

    /// <summary>
    ///     Tap into the value of an asynchronous <see cref="ResultType{T}" /> to
    ///     invoke a side effect.
    /// </summary>
    /// <param name="resultTask">The <see cref="ResultType{T}" /></param>
    /// <param name="tap">The side effect</param>
    /// <param name="tapExpression">The side effect expression string</param>
    /// <typeparam name="TIn">The input result's value type</typeparam>
    /// <returns>The input result</returns>
    [DebuggerStepperBoundary]
    public static async Task<ResultType<TIn>> TapAsync<TIn>(
        this Task<ResultType<TIn>> resultTask,
        Action<TIn> tap,
        [CallerArgumentExpression(nameof(tap))]
        string tapExpression = "") =>
        await resultTask.PipeAsync(
            async value =>
            {
                try
                {
                    tap(value);
                    return await resultTask;
                }
                catch (Exception ex)
                {
                    return await Result.ErrorAsync<TIn>(
                        MonadErrors.FailedToTapValue(ex, tapExpression));
                }
            });
}
