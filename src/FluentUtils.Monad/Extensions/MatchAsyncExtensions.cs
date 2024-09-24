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
        Func<TIn, Task<TOut>> okHandler,
        Func<Error, Task<TOut>> errorHandler
    )
    {
        return await resultTask switch
        {
            OkResultType<TIn> ok => await okHandler(ok.Value),
            ErrorResultType<TIn> err => await errorHandler(err.Error),
            var result => throw new UnsupportedResultTypeException<TIn>(result),
        };
    }

    /// <summary>
    /// </summary>
    /// <param name="resultTask"></param>
    /// <param name="okHandler"></param>
    /// <param name="errorHandler"></param>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<TOut> MatchAsync<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, TOut> okHandler,
        Func<Error, TOut> errorHandler) => await resultTask.MatchAsync(
        value => Task.FromResult(okHandler(value)),
        error => Task.FromResult(errorHandler(error)));

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
    /// <typeparam name="TOut">The output value's type</typeparam>
    /// <returns>The output value from the invoked handler</returns>
    /// <exception cref="UnsupportedResultTypeException{TIn}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static async Task<TOut> MatchAsync<TOut>(
        this Task<ResultType<Empty>> resultTask,
        Func<Task<TOut>> okHandler,
        Func<Error, Task<TOut>> errorHandler
    ) => await resultTask.MatchAsync(
        _ => okHandler(),
        errorHandler
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
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <returns>The completed task</returns>
    /// <exception cref="UnsupportedResultTypeException{TIn}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static async Task MatchAsync<TIn>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, Task> okHandler,
        Func<Error, Task> errorHandler
    )
    {
        await resultTask.MatchAsync<TIn, Empty>(
            async value =>
            {
                await okHandler(value);
                return default;
            },
            async error =>
            {
                await errorHandler(error);
                return default;
            }
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
    /// <returns>The completed task</returns>
    /// <exception cref="UnsupportedResultTypeException{TIn}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static async Task MatchAsync(
        this Task<ResultType<Empty>> resultTask,
        Func<Task> okHandler,
        Func<Error, Task> errorHandler
    ) => await resultTask.MatchAsync(
        _ => okHandler(),
        errorHandler
    );
}
