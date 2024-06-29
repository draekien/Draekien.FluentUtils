namespace FluentUtils.Monad.Extensions;

/// <summary>
///     Extensions for mapping the value of an asynchronous
///     <see cref="ResultType{T}" /> to another type
/// </summary>
[PublicAPI]
public static class MapAsyncExtensions
{
    /// <summary>
    ///     Maps the value of an asynchronous <see cref="ResultType{T}" /> to another
    ///     type if the <see cref="ResultType{T}" />
    ///     is an <see cref="OkResultType{T}" />, otherwise maps the
    ///     <see cref="ErrorResultType{T}" /> to the target type
    /// </summary>
    /// <param name="result">The asynchronous <see cref="ResultType{T}" /></param>
    /// <param name="mapAsync">The map operation</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <typeparam name="TOut">The output result value's type</typeparam>
    /// <returns>
    ///     A <see cref="ResultType{T}" /> containing the mapped value, or the
    ///     forwarded <see cref="ErrorResultType{T}" />
    /// </returns>
    public static async Task<ResultType<TOut>> MapAsync<TIn, TOut>(
        this Task<ResultType<TIn>> result,
        Func<TIn, CancellationToken, Task<ResultType<TOut>>> mapAsync,
        CancellationToken cancellationToken = default
    )
        where TIn : notnull where TOut : notnull => await result.MatchAsync(
        mapAsync,
        Result.ErrorAsync<TOut>,
        cancellationToken
    );
}
