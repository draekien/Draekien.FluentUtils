using JetBrains.Annotations;

namespace FluentUtils.Monad;

/// <summary>
///     Extension methods for working with <see cref="IResult{T}" />
/// </summary>
[PublicAPI]
public static class ResultExtensions
{
    private const string UnexpectedResultType = "Unexpected result type encountered.";

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result contains a value
    /// </remarks>
    /// <param name="result">The <see cref="IResult{T}" /></param>
    /// <param name="okHandler">The handler to execute for an <see cref="OkResult{T}" /></param>
    /// <param name="errorHandler">The handler to execute for an <see cref="ErrorResult{T}" /></param>
    /// <typeparam name="TIn">The value type contained in the <see cref="OkResult{T}" /></typeparam>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static TOut Match<TIn, TOut>(this IResult<TIn> result, Func<TIn, TOut> okHandler,
        Func<Error, TOut> errorHandler) where TIn : notnull
    {
        return result switch
        {
            OkResult<TIn> ok => okHandler(ok.Value),
            ErrorResult<TIn> err => errorHandler(err.Error),
            _ => throw new InvalidOperationException(UnexpectedResultType)
        };
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result contains an empty value (e.g. void)
    /// </remarks>
    /// <param name="result">The <see cref="IResult{T}" /></param>
    /// <param name="okHandler">The handler to execute for an <see cref="OkResult{T}" /></param>
    /// <param name="errorHandler">The handler to execute for an <see cref="ErrorResult{T}" /></param>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static TOut Match<TOut>(this IResult<Empty> result, Func<TOut> okHandler, Func<Error, TOut> errorHandler)
    {
        return result.Match(_ => okHandler(), errorHandler);
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your ersult contains an empty value (e.g. void) and your handlers return void
    /// </remarks>
    /// <param name="result">The <see cref="IResult{T}" /></param>
    /// <param name="okHandler">The handler to execute for an <see cref="OkResult{T}" /></param>
    /// <param name="errorHandler">The handler to execute for an <see cref="ErrorResult{T}" /></param>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static void Match(this IResult<Empty> result, Action okHandler, Action<Error> errorHandler)
    {
        switch (result)
        {
            case OkResult<Empty>:
                okHandler();
                break;
            case ErrorResult<Empty> err:
                errorHandler(err.Error);
                break;
            default:
                throw new InvalidOperationException(UnexpectedResultType);
        }
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains some value but your handlers are synchronous
    /// </remarks>
    /// <param name="resultTask">A task which when resolved returns a <see cref="IResult{T}" /></param>
    /// <param name="okHandler">The handler to execute for an <see cref="OkResult{T}" /></param>
    /// <param name="errorHandler">The handler to execute for an <see cref="ErrorResult{T}" /></param>
    /// <typeparam name="TIn">The value type contained in the <see cref="OkResult{T}" /></typeparam>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<IResult<TIn>> resultTask, Func<TIn, TOut> okHandler,
        Func<Error, TOut> errorHandler) where TIn : notnull
    {
        return (await resultTask).Match(okHandler, errorHandler);
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains an empty value but your handlers are synchronous
    /// </remarks>
    /// <param name="resultTask">A task which when resolved returns a <see cref="IResult{T}" /></param>
    /// <param name="okHandler">The handler to execute for an <see cref="OkResult{T}" /></param>
    /// <param name="errorHandler">The handler to execute for an <see cref="ErrorResult{T}" /></param>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static Task<TOut> MatchAsync<TOut>(this Task<IResult<Empty>> resultTask, Func<TOut> okHandler,
        Func<Error, TOut> errorHandler)
    {
        return resultTask.MatchAsync(_ => okHandler(), errorHandler);
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains an empty value but your handlers are synchronous and
    ///     return no value
    /// </remarks>
    /// <param name="resultTask">A task which when resolved returns a <see cref="IResult{T}" /></param>
    /// <param name="okHandler">The handler to execute for an <see cref="OkResult{T}" /></param>
    /// <param name="errorHandler">The handler to execute for an <see cref="ErrorResult{T}" /></param>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static async Task MatchAsync(this Task<IResult<Empty>> resultTask, Action okHandler,
        Action<Error> errorHandler)
    {
        (await resultTask).Match(okHandler, errorHandler);
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains some value, your handlers are asynchronous, and your
    ///     handlers support cancellation tokens
    /// </remarks>
    /// <param name="resultTask">A task which when resolved returns a <see cref="IResult{T}" /></param>
    /// <param name="okHandler">The handler to execute for an <see cref="OkResult{T}" /></param>
    /// <param name="errorHandler">The handler to execute for an <see cref="ErrorResult{T}" /></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <typeparam name="TIn">The value type contained in the <see cref="OkResult{T}" /></typeparam>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<IResult<TIn>> resultTask,
        Func<TIn, CancellationToken, Task<TOut>> okHandler, Func<Error, CancellationToken, Task<TOut>> errorHandler,
        CancellationToken cancellationToken) where TIn : notnull
    {
        return await resultTask switch
        {
            OkResult<TIn> ok => await okHandler(ok.Value, cancellationToken),
            ErrorResult<TIn> err => await errorHandler(err.Error, cancellationToken),
            _ => throw new InvalidOperationException(UnexpectedResultType)
        };
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains some value, your handlers are asynchronous, and your
    ///     handlers do not support cancellation tokens
    /// </remarks>
    /// <param name="resultTask">A task which when resolved returns a <see cref="IResult{T}" /></param>
    /// <param name="okHandler">The handler to execute for an <see cref="OkResult{T}" /></param>
    /// <param name="errorHandler">The handler to execute for an <see cref="ErrorResult{T}" /></param>
    /// <typeparam name="TIn">The value type contained in the <see cref="OkResult{T}" /></typeparam>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static Task<TOut> MatchAsync<TIn, TOut>(this Task<IResult<TIn>> resultTask,
        Func<TIn, Task<TOut>> okHandler, Func<Error, Task<TOut>> errorHandler) where TIn : notnull
    {
        return resultTask.MatchAsync((value, _) => okHandler(value), (err, _) => errorHandler(err),
            CancellationToken.None);
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains no value, your handlers are asynchronous, and your
    ///     handlers support cancellation tokens
    /// </remarks>
    /// <param name="resultTask">A task which when resolved returns a <see cref="IResult{T}" /></param>
    /// <param name="okHandler">The handler to execute for an <see cref="OkResult{T}" /></param>
    /// <param name="errorHandler">The handler to execute for an <see cref="ErrorResult{T}" /></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static Task<TOut> MatchAsync<TOut>(this Task<IResult<Empty>> resultTask,
        Func<CancellationToken, Task<TOut>> okHandler, Func<Error, CancellationToken, Task<TOut>> errorHandler,
        CancellationToken cancellationToken)
    {
        return resultTask.MatchAsync((_, ct) => okHandler(ct), errorHandler,
            cancellationToken);
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains no value, your handlers are asynchronous, and your
    ///     handlers do not support cancellation tokens
    /// </remarks>
    /// <param name="resultTask">A task which when resolved returns a <see cref="IResult{T}" /></param>
    /// <param name="okHandler">The handler to execute for an <see cref="OkResult{T}" /></param>
    /// <param name="errorHandler">The handler to execute for an <see cref="ErrorResult{T}" /></param>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static Task<TOut> MatchAsync<TOut>(this Task<IResult<Empty>> resultTask,
        Func<Task<TOut>> okHandler, Func<Error, Task<TOut>> errorHandler)
    {
        return resultTask.MatchAsync((_, _) => okHandler(), (err, _) => errorHandler(err),
            CancellationToken.None);
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains no value, your handlers are asynchronous, your handlers
    ///     support cancellation tokens, and your handlers return no value
    /// </remarks>
    /// <param name="resultTask">A task which when resolved returns a <see cref="IResult{T}" /></param>
    /// <param name="okHandler">The handler to execute for an <see cref="OkResult{T}" /></param>
    /// <param name="errorHandler">The handler to execute for an <see cref="ErrorResult{T}" /></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static async Task MatchAsync(this Task<IResult<Empty>> resultTask, Func<CancellationToken, Task> okHandler,
        Func<Error, CancellationToken, Task> errorHandler, CancellationToken cancellationToken)
    {
        switch (await resultTask)
        {
            case OkResult<Empty>:
                await okHandler(cancellationToken);
                break;
            case ErrorResult<Empty> err:
                await errorHandler(err.Error, cancellationToken);
                break;
            default:
                throw new InvalidOperationException(UnexpectedResultType);
        }
    }

    /// <summary>
    ///     Returns the <see cref="IResult{T}" />'s contained OK value
    /// </summary>
    /// <remarks>
    ///     It is preferable to use <c>Match</c> to handle the error case explicitly
    /// </remarks>
    /// <param name="result">The <see cref="IResult{T}" /></param>
    /// <typeparam name="T">The value type associated with an <see cref="OkResult{T}" /></typeparam>
    /// <returns>The value type</returns>
    /// <exception cref="UnwrapPanicException">The result is an error variant</exception>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static T Unwrap<T>(this IResult<T> result) where T : notnull
    {
        return result switch
        {
            OkResult<T> ok => ok.Value,
            ErrorResult<T> err => throw new UnwrapPanicException(err.Error),
            _ => throw new InvalidOperationException(UnexpectedResultType)
        };
    }

    /// <summary>
    ///     Waits for the task to complete, then returns the <see cref="IResult{T}" />'s contained OK value
    /// </summary>
    /// <param name="resultTask">A task which when completed will return a <see cref="IResult{T}" /></param>
    /// <typeparam name="T">The value type associated with an <see cref="OkResult{T}" /></typeparam>
    /// <returns>The value type</returns>
    /// <exception cref="UnwrapPanicException">The result is an error variant</exception>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static async Task<T> UnwrapAsync<T>(this Task<IResult<T>> resultTask) where T : notnull
    {
        return (await resultTask).Unwrap();
    }
}