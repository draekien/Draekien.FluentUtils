namespace FluentUtils.Monad.Extensions;

/// <summary>
///     Extensions for matching on an asynchronous <see cref="ResultType{T}" />
/// </summary>
[PublicAPI]
public static class MatchAsyncExtensions
{
    /// <summary>
    ///     Performs a match on the <see cref="ResultType{T}" />, invoking the
    ///     <see cref="okHandler" /> for an <see cref="OkResultType{T}" />
    ///     and the <see cref="errorHandler" /> for an
    ///     <see cref="ErrorResultType{T}" />
    /// </summary>
    /// <param name="resultTask">The asynchronous <see cref="ResultType{T}" /></param>
    /// <param name="okHandler">
    ///     The operation to perform for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The operation to perform for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <typeparam name="TOut">The output value's type</typeparam>
    /// <returns>The output value from the invoked handler</returns>
    /// <exception cref="UnsupportedResultTypeException{TIn}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static async Task<TOut> MatchAsync<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, CancellationToken, Task<TOut>> okHandler,
        Func<Error, CancellationToken, Task<TOut>> errorHandler,
        CancellationToken cancellationToken = default
    )
    {
        return await resultTask switch
        {
            OkResultType<TIn> ok => await okHandler(
                ok.Value,
                cancellationToken
            ),
            ErrorResultType<TIn> err => await errorHandler(
                err.Error,
                cancellationToken
            ),
            var result => throw new UnsupportedResultTypeException<TIn>(result),
        };
    }

    /// <summary>
    ///     Performs a match on the <see cref="ResultType{T}" />, invoking the
    ///     <see cref="okHandler" /> for an <see cref="OkResultType{T}" />
    ///     and the <see cref="errorHandler" /> for an
    ///     <see cref="ErrorResultType{T}" />
    /// </summary>
    /// <param name="resultTask">The asynchronous <see cref="ResultType{T}" /></param>
    /// <param name="okHandler">
    ///     The operation to perform for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The operation to perform for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <typeparam name="TOut">The output value's type</typeparam>
    /// <returns>The output value from the invoked handler</returns>
    /// <exception cref="UnsupportedResultTypeException{TIn}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static async Task<TOut> MatchAsync<TOut>(
        this Task<ResultType<Empty>> resultTask,
        Func<CancellationToken, Task<TOut>> okHandler,
        Func<Error, CancellationToken, Task<TOut>> errorHandler,
        CancellationToken cancellationToken = default
    ) => await resultTask.MatchAsync(
        (_, token) => okHandler(token),
        errorHandler,
        cancellationToken
    );

    /// <summary>
    ///     Performs a match on the <see cref="ResultType{T}" />, invoking the
    ///     <see cref="okHandler" /> for an <see cref="OkResultType{T}" />
    ///     and the <see cref="errorHandler" /> for an
    ///     <see cref="ErrorResultType{T}" />
    /// </summary>
    /// <param name="resultTask">The asynchronous <see cref="ResultType{T}" /></param>
    /// <param name="okHandler">
    ///     The operation to perform for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The operation to perform for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <returns>The completed task</returns>
    /// <exception cref="UnsupportedResultTypeException{TIn}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static async Task MatchAsync<TIn>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, CancellationToken, Task> okHandler,
        Func<Error, CancellationToken, Task> errorHandler,
        CancellationToken cancellationToken = default
    )
    {
        await resultTask.MatchAsync<TIn, Empty>(
            async (value, token) =>
            {
                await okHandler(value, token);
                return default;
            },
            async (error, token) =>
            {
                await errorHandler(error, token);
                return default;
            },
            cancellationToken
        );
    }

    /// <summary>
    ///     Performs a match on the <see cref="ResultType{T}" />, invoking the
    ///     <see cref="okHandler" /> for an <see cref="OkResultType{T}" />
    ///     and the <see cref="errorHandler" /> for an
    ///     <see cref="ErrorResultType{T}" />
    /// </summary>
    /// <param name="resultTask">The asynchronous <see cref="ResultType{T}" /></param>
    /// <param name="okHandler">
    ///     The operation to perform for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The operation to perform for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <returns>The completed task</returns>
    /// <exception cref="UnsupportedResultTypeException{TIn}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static async Task MatchAsync(
        this Task<ResultType<Empty>> resultTask,
        Func<CancellationToken, Task> okHandler,
        Func<Error, CancellationToken, Task> errorHandler,
        CancellationToken cancellationToken = default
    ) => await resultTask.MatchAsync(
        (_, token) => okHandler(token),
        errorHandler,
        cancellationToken
    );
}
