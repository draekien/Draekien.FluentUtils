namespace FluentUtils.Monad;

using JetBrains.Annotations;

/// <summary>
///     Extension methods for working with <see cref="ResultType{T}" />
/// </summary>
[PublicAPI]
public static class ResultExtensions
{
    private const string UnexpectedResultType =
        "Unexpected result type encountered.";

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result contains a value
    /// </remarks>
    /// <param name="resultType">The <see cref="ResultType{T}" /></param>
    /// <param name="okHandler">
    ///     The handler to execute for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The handler to execute for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <typeparam name="TIn">
    ///     The value type contained in the
    ///     <see cref="OkResultType{T}" />
    /// </typeparam>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static TOut Match<TIn, TOut>(
        this ResultType<TIn> resultType,
        Func<TIn, TOut> okHandler,
        Func<Error, TOut> errorHandler
    ) where TIn : notnull
    {
        return resultType switch
        {
            OkResultType<TIn> ok => okHandler(ok.Value),
            ErrorResultType<TIn> err => errorHandler(err.Error),
            _ => throw new InvalidOperationException(UnexpectedResultType),
        };
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your handlers return void
    /// </remarks>
    /// <param name="resultType">The <see cref="ResultType{T}" /></param>
    /// <param name="okHandler">
    ///     The handler to execute for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The handler to execute for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <typeparam name="T">
    ///     The value type contained in the
    ///     <see cref="OkResultType{T}" />
    /// </typeparam>
    public static void Match<T>(
        this ResultType<T> resultType,
        Action<T> okHandler,
        Action<Error> errorHandler
    ) where T : notnull
    {
        resultType.Match<T, Empty>(
            value =>
            {
                okHandler(value);
                return default;
            },
            error =>
            {
                errorHandler(error);
                return default;
            }
        );
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result contains an empty value (e.g. void)
    /// </remarks>
    /// <param name="resultType">The <see cref="ResultType{T}" /></param>
    /// <param name="okHandler">
    ///     The handler to execute for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The handler to execute for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static TOut Match<TOut>(
        this ResultType<Empty> resultType,
        Func<TOut> okHandler,
        Func<Error, TOut> errorHandler
    )
    {
        return resultType.Match(_ => okHandler(), errorHandler);
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your ersult contains an empty value (e.g. void) and
    ///     your handlers return void
    /// </remarks>
    /// <param name="resultType">The <see cref="ResultType{T}" /></param>
    /// <param name="okHandler">
    ///     The handler to execute for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The handler to execute for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static void Match(
        this ResultType<Empty> resultType,
        Action okHandler,
        Action<Error> errorHandler
    )
    {
        switch (resultType)
        {
            case OkResultType<Empty>:
                okHandler();
                break;
            case ErrorResultType<Empty> err:
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
    ///     Use this overload when your result is a task that contains some value but
    ///     your handlers are synchronous
    /// </remarks>
    /// <param name="resultTask">
    ///     A task which when resolved returns a
    ///     <see cref="ResultType{T}" />
    /// </param>
    /// <param name="okHandler">
    ///     The handler to execute for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The handler to execute for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <typeparam name="TIn">
    ///     The value type contained in the
    ///     <see cref="OkResultType{T}" />
    /// </typeparam>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static async Task<TOut> MatchAsync<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, TOut> okHandler,
        Func<Error, TOut> errorHandler
    ) where TIn : notnull =>
        (await resultTask).Match(okHandler, errorHandler);

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains an empty value
    ///     but your handlers are synchronous
    /// </remarks>
    /// <param name="resultTask">
    ///     A task which when resolved returns a
    ///     <see cref="ResultType{T}" />
    /// </param>
    /// <param name="okHandler">
    ///     The handler to execute for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The handler to execute for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static Task<TOut> MatchAsync<TOut>(
        this Task<ResultType<Empty>> resultTask,
        Func<TOut> okHandler,
        Func<Error, TOut> errorHandler
    ) => resultTask.MatchAsync(_ => okHandler(), errorHandler);

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains an empty value
    ///     but your handlers are synchronous and
    ///     return no value
    /// </remarks>
    /// <param name="resultTask">
    ///     A task which when resolved returns a
    ///     <see cref="ResultType{T}" />
    /// </param>
    /// <param name="okHandler">
    ///     The handler to execute for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The handler to execute for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static async Task MatchAsync(
        this Task<ResultType<Empty>> resultTask,
        Action okHandler,
        Action<Error> errorHandler
    ) =>
        (await resultTask).Match(okHandler, errorHandler);

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains some value, your
    ///     handlers are asynchronous, and your
    ///     handlers support cancellation tokens
    /// </remarks>
    /// <param name="resultTask">
    ///     A task which when resolved returns a
    ///     <see cref="ResultType{T}" />
    /// </param>
    /// <param name="okHandler">
    ///     The handler to execute for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The handler to execute for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <typeparam name="TIn">
    ///     The value type contained in the
    ///     <see cref="OkResultType{T}" />
    /// </typeparam>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static async Task<TOut> MatchAsync<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, CancellationToken, Task<TOut>> okHandler,
        Func<Error, CancellationToken, Task<TOut>> errorHandler,
        CancellationToken cancellationToken
    ) where TIn : notnull
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
            _ => throw new InvalidOperationException(UnexpectedResultType),
        };
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains some value, your
    ///     handlers are asynchronous, and your
    ///     handlers do not support cancellation tokens
    /// </remarks>
    /// <param name="resultTask">
    ///     A task which when resolved returns a
    ///     <see cref="ResultType{T}" />
    /// </param>
    /// <param name="okHandler">
    ///     The handler to execute for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The handler to execute for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <typeparam name="TIn">
    ///     The value type contained in the
    ///     <see cref="OkResultType{T}" />
    /// </typeparam>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static Task<TOut> MatchAsync<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, Task<TOut>> okHandler,
        Func<Error, Task<TOut>> errorHandler
    ) where TIn : notnull
    {
        return resultTask.MatchAsync(
            (value, _) => okHandler(value),
            (err, _) => errorHandler(err),
            CancellationToken.None
        );
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains no value, your
    ///     handlers are asynchronous, and your
    ///     handlers support cancellation tokens
    /// </remarks>
    /// <param name="resultTask">
    ///     A task which when resolved returns a
    ///     <see cref="ResultType{T}" />
    /// </param>
    /// <param name="okHandler">
    ///     The handler to execute for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The handler to execute for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static Task<TOut> MatchAsync<TOut>(
        this Task<ResultType<Empty>> resultTask,
        Func<CancellationToken, Task<TOut>> okHandler,
        Func<Error, CancellationToken, Task<TOut>> errorHandler,
        CancellationToken cancellationToken
    )
    {
        return resultTask.MatchAsync(
            (_, ct) => okHandler(ct),
            errorHandler,
            cancellationToken
        );
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains no value, your
    ///     handlers are asynchronous, and your
    ///     handlers do not support cancellation tokens
    /// </remarks>
    /// <param name="resultTask">
    ///     A task which when resolved returns a
    ///     <see cref="ResultType{T}" />
    /// </param>
    /// <param name="okHandler">
    ///     The handler to execute for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The handler to execute for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <typeparam name="TOut">The output value type</typeparam>
    /// <returns>The output value</returns>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static Task<TOut> MatchAsync<TOut>(
        this Task<ResultType<Empty>> resultTask,
        Func<Task<TOut>> okHandler,
        Func<Error, Task<TOut>> errorHandler
    )
    {
        return resultTask.MatchAsync(
            (_, _) => okHandler(),
            (err, _) => errorHandler(err),
            CancellationToken.None
        );
    }

    /// <summary>
    ///     Checks the result variant and invokes the matching result handler
    /// </summary>
    /// <remarks>
    ///     Use this overload when your result is a task that contains no value, your
    ///     handlers are asynchronous, your handlers
    ///     support cancellation tokens, and your handlers return no value
    /// </remarks>
    /// <param name="resultTask">
    ///     A task which when resolved returns a
    ///     <see cref="ResultType{T}" />
    /// </param>
    /// <param name="okHandler">
    ///     The handler to execute for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The handler to execute for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static async Task MatchAsync(
        this Task<ResultType<Empty>> resultTask,
        Func<CancellationToken, Task> okHandler,
        Func<Error, CancellationToken, Task> errorHandler,
        CancellationToken cancellationToken
    )
    {
        switch (await resultTask)
        {
            case OkResultType<Empty>:
                await okHandler(cancellationToken);
                break;
            case ErrorResultType<Empty> err:
                await errorHandler(err.Error, cancellationToken);
                break;
            default:
                throw new InvalidOperationException(UnexpectedResultType);
        }
    }

    /// <summary>
    ///     Returns the <see cref="ResultType{T}" />'s contained OK value
    /// </summary>
    /// <remarks>
    ///     It is preferable to use <c>Match</c> to handle the error case explicitly
    /// </remarks>
    /// <param name="resultType">The <see cref="ResultType{T}" /></param>
    /// <typeparam name="T">
    ///     The value type associated with an
    ///     <see cref="OkResultType{T}" />
    /// </typeparam>
    /// <returns>The value type</returns>
    /// <exception cref="UnwrapPanicException">The result is an error variant</exception>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static T Unwrap<T>(this ResultType<T> resultType) where T : notnull
    {
        return resultType switch
        {
            OkResultType<T> ok => ok.Value,
            ErrorResultType<T> err => throw new UnwrapPanicException(err.Error),
            _ => throw new InvalidOperationException(UnexpectedResultType),
        };
    }

    /// <summary>
    ///     Waits for the task to complete, then returns the
    ///     <see cref="ResultType{T}" />'s contained OK value
    /// </summary>
    /// <param name="resultTask">
    ///     A task which when completed will return a
    ///     <see cref="ResultType{T}" />
    /// </param>
    /// <typeparam name="T">
    ///     The value type associated with an
    ///     <see cref="OkResultType{T}" />
    /// </typeparam>
    /// <returns>The value type</returns>
    /// <exception cref="UnwrapPanicException">The result is an error variant</exception>
    /// <exception cref="InvalidOperationException">The result is an unexpected variant</exception>
    public static async Task<T> UnwrapAsync<T>(
        this Task<ResultType<T>> resultTask
    ) where T : notnull => (await resultTask).Unwrap();

    /// <summary>
    ///     Maps a result's value to a different type
    /// </summary>
    /// <remarks>
    ///     The mapper is only executed if the <see cref="result" /> is ok
    /// </remarks>
    /// <param name="result">The <see cref="ResultType{T}" /></param>
    /// <param name="map">The <see cref="map" /> to execute</param>
    /// <typeparam name="TIn">The type of the original result</typeparam>
    /// <typeparam name="TOut">The output value's type</typeparam>
    /// <returns>A <see cref="ResultType{T}" /> containing the mapped value</returns>
    public static ResultType<TOut> Map<TIn, TOut>(
        this ResultType<TIn> result,
        Func<TIn, ResultType<TOut>> map
    ) where TIn : notnull where TOut : notnull =>
        result.Match(
            map,
            Result.Error<TOut>
        );

    /// <summary>
    ///     Maps an asynchronous result's value to a different type
    /// </summary>
    /// <param name="resultTask">
    ///     A task which when awaited returns a
    ///     <see cref="ResultType{T}" />
    /// </param>
    /// <param name="mapAsync">The async mapper</param>
    /// <typeparam name="TIn">The type of the original result</typeparam>
    /// <typeparam name="TOut">The output value's type</typeparam>
    /// <returns>A task which when awaited returns a result containing the mapped value</returns>
    public static Task<ResultType<TOut>> MapAsync<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, Task<ResultType<TOut>>> mapAsync
    ) where TIn : notnull where TOut : notnull =>
        resultTask.MatchAsync(
            mapAsync,
            error => Task.FromResult(Result.Error<TOut>(error))
        );
}
