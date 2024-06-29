namespace FluentUtils.Monad.Extensions;

/// <summary>
///     Extensions for unwrapping the value of an asynchronous
///     <see cref="ResultType{T}" />
/// </summary>
[PublicAPI]
public static class UnwrapAsyncExtensions
{
    /// <summary>
    ///     Unwraps the value from an asynchronous <see cref="ResultType{T}" /> without
    ///     checking if
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
    /// <exception cref="UnsupportedResultTypeException{T}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static async Task<T> UnwrapAsync<T>(this Task<ResultType<T>> result)
        where T : notnull => (await result).Unwrap();
}
