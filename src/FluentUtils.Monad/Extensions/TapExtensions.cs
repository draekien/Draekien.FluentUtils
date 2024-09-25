namespace FluentUtils.Monad.Extensions;

using System.Diagnostics;
using System.Runtime.CompilerServices;

/// <summary>
///     Extensions for tapping into the value of a result without modifying it.
/// </summary>
public static class TapExtensions
{
    /// <summary>
    ///     Access the value of a <see cref="ResultType{T}" /> in order to perform a
    ///     side effect.
    /// </summary>
    /// <param name="result">The <see cref="ResultType{T}" /></param>
    /// <param name="tap">The tap operation</param>
    /// <param name="tapExpression">The string representation of the tap operation</param>
    /// <typeparam name="TIn">
    ///     The value of the input <see cref="ResultType{T}" />
    /// </typeparam>
    /// <returns>The input <see cref="ResultType{T}" /></returns>
    [DebuggerStepperBoundary]
    public static ResultType<TIn> Tap<TIn>(
        this ResultType<TIn> result,
        Action<TIn> tap,
        [CallerArgumentExpression(nameof(tap))]
        string tapExpression = "") => result.Pipe(
        value =>
        {
            try
            {
                tap(value);
                return Result.Ok(value);
            }
            catch (Exception ex)
            {
                return Result.Error<TIn>(
                    MonadErrors.FailedToTapValue(ex, tapExpression));
            }
        });
}
