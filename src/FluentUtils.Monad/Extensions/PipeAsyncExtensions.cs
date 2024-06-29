namespace FluentUtils.Monad.Extensions;

/// <summary>
///     Extensions for mapping the value of an asynchronous
///     <see cref="ResultType{T}" /> to another type
/// </summary>
[PublicAPI]
public static class PipeAsyncExtensions
{
    /// <summary>
    ///     Pipes the value of an asynchronous <see cref="ResultType{T}" /> to another
    ///     type if the <see cref="ResultType{T}" />
    ///     is an <see cref="OkResultType{T}" />, otherwise pipes the
    ///     <see cref="ErrorResultType{T}" /> to the target type
    /// </summary>
    /// <param name="result">The asynchronous <see cref="ResultType{T}" /></param>
    /// <param name="pipeAsync">The pipe operation</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <typeparam name="TOut">The output result value's type</typeparam>
    /// <returns>
    ///     A <see cref="ResultType{T}" /> containing the piped value, or the
    ///     forwarded <see cref="ErrorResultType{T}" />
    /// </returns>
    public static async Task<ResultType<TOut>> PipeAsync<TIn, TOut>(
        this Task<ResultType<TIn>> result,
        Func<TIn, CancellationToken, Task<ResultType<TOut>>> pipeAsync,
        CancellationToken cancellationToken = default
    )
        where TIn : notnull where TOut : notnull => await result.MatchAsync(
        pipeAsync,
        Result.ErrorAsync<TOut>,
        cancellationToken
    );
}
