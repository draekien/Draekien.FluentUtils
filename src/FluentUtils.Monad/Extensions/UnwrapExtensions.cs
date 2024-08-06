namespace FluentUtils.Monad.Extensions;

/// <summary>
///     Extensions for unwrapping the value of a <see cref="ResultType{T}" />
/// </summary>
[PublicAPI]
public static class UnwrapExtensions
{
    /// <summary>
    ///     Unwraps the value from a <see cref="ResultType{T}" /> without checking if
    ///     the result is an <see cref="ErrorResultType{T}" />
    /// </summary>
    /// <param name="result">The <see cref="ResultType{T}" /></param>
    /// <typeparam name="T">The result value's type</typeparam>
    /// <returns>The result's value</returns>
    /// <exception cref="UnwrapPanicException">
    ///     Failed to unwrap the value of the
    ///     <see cref="ResultType{T}" /> because it is an instance of
    ///     <see cref="ErrorResultType{T}" />
    /// </exception>
    /// <exception cref="UnsupportedResultTypeException{TIn}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static T Unwrap<T>(this ResultType<T> result) =>
        result switch
        {
            OkResultType<T> ok => ok.Value,
            ErrorResultType<T> err => throw new UnwrapPanicException(err.Error),
            var _ => throw new UnsupportedResultTypeException<T>(result),
        };
}
