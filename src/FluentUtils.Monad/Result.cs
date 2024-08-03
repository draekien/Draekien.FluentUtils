namespace FluentUtils.Monad;

using System.Runtime.CompilerServices;

/// <summary>
///     A result is the type used for returning and propagating errors. It is a
///     monad with the variants
///     <see cref="OkResultType{T}" />, representing success and containing some
///     value, and
///     <see cref="ErrorResultType{T}" />,
///     representing error and containing an error value.
/// </summary>
[PublicAPI]
public static class Result
{
    /// <summary>
    ///     Creates a <see cref="Task" /> that contains a success result variant
    /// </summary>
    /// <param name="value">The success value</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <typeparam name="T">The value type</typeparam>
    /// <returns>The created result</returns>
    internal static Task<ResultType<T>> OkAsync<T>(
        T value,
        CancellationToken cancellationToken = default
    ) =>
        Task.FromResult<ResultType<T>>(new OkResultType<T>(value));

    /// <summary>
    ///     Creates a <see cref="Task" /> that contains a success result variant which
    ///     holds no value
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <returns>The created result</returns>
    internal static Task<ResultType<Empty>> OkAsync(
        CancellationToken cancellationToken = default
    ) => OkAsync<Empty>(default, cancellationToken);

    /// <summary>
    ///     Creates a success result variant
    /// </summary>
    /// <param name="value">The success value</param>
    /// <typeparam name="T">The value type</typeparam>
    /// <returns>The created result</returns>
    public static ResultType<T> Ok<T>(T value) =>
        new OkResultType<T>(value);

    /// <summary>
    ///     Creates a successful result variant which holds no value
    /// </summary>
    /// <returns>The created result</returns>
    public static ResultType<Empty> Ok() => Ok<Empty>(default);

    /// <summary>
    ///     Binds the result of a factory method to a <see cref="ResultType{T}" />
    /// </summary>
    /// <param name="factory">The factory method</param>
    /// <typeparam name="T">The factory output type</typeparam>
    /// <returns>A <see cref="ResultType{T}" /></returns>
    public static ResultType<T> Bind<T>(Func<T> factory)
    {
        try
        {
            return factory();
        }
        catch (Exception ex)
        {
            return MonadErrors.FailedToBindFactory(ex);
        }
    }

    /// <summary>
    ///     Binds the results of an asynchronous factory method to a
    ///     <see cref="ResultType{T}" />
    /// </summary>
    /// <param name="factory">The async factory</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <typeparam name="T">The factory output type</typeparam>
    /// <returns>
    ///     A task which when awaited will return a <see cref="ResultType{T}" />
    /// </returns>
    public static async Task<ResultType<T>> BindAsync<T>(
        Func<CancellationToken, Task<T>> factory,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await factory(cancellationToken);
        }
        catch (Exception ex)
        {
            return MonadErrors.FailedToBindFactory(ex);
        }
    }

    /// <summary>
    ///     Creates a <see cref="Task" /> that contains an error result variant
    /// </summary>
    /// <param name="error">The <see cref="Error(FluentUtils.Monad.Error)" /></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <typeparam name="T">
    ///     The value type associated with its success result
    ///     counterpart
    /// </typeparam>
    /// <returns>The <see cref="ResultType{T}" /> <see cref="Task" /></returns>
    internal static Task<ResultType<T>> ErrorAsync<T>(
        Error error,
        CancellationToken cancellationToken = default
    )
        =>
            Task.FromResult<ResultType<T>>(new ErrorResultType<T>(error));

    /// <summary>
    ///     Creates a <see cref="Task" /> that contains an error result variant
    /// </summary>
    /// <param name="error">The <see cref="Error(FluentUtils.Monad.Error)" /></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
    /// <returns>The created result</returns>
    internal static Task<ResultType<Empty>> ErrorAsync(
        Error error,
        CancellationToken cancellationToken = default
    ) => ErrorAsync<Empty>(error, cancellationToken);

    /// <summary>
    ///     Creates an error result variant
    /// </summary>
    /// <param name="error">The error value</param>
    /// <typeparam name="T">
    ///     The value type associated with its success result
    ///     counterpart
    /// </typeparam>
    /// <returns>The created result</returns>
    public static ResultType<T> Error<T>(Error error) =>
        new ErrorResultType<T>(error);

    /// <summary>
    ///     Creates an error result variant which is the counterpart to a success
    ///     result that holds no value
    /// </summary>
    /// <param name="error">The error value</param>
    /// <returns>The created result</returns>
    public static ResultType<Empty> Error(Error error) =>
        Error<Empty>(error);

    /// <summary>
    ///     Creates an error result variant from a message and an optional exception.
    /// </summary>
    /// <remarks>
    ///     This overload dynamically generates the error code using the name of the
    ///     calling method / property
    ///     and the line number where this overload is invoked.
    /// </remarks>
    /// <param name="message">The error message to capture inside the result</param>
    /// <param name="exception">Optional. The exception to capture inside the result</param>
    /// <param name="caller">The caller member name</param>
    /// <param name="lineNumber">The caller line number</param>
    /// <typeparam name="T">
    ///     The value type associated with its success result
    ///     counterpart
    /// </typeparam>
    /// <returns>The created result</returns>
    public static ResultType<T> Error<T>(
        string message,
        Exception? exception = default,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int lineNumber = 0
    ) => CreateError(
        CreateCode(caller, lineNumber),
        message,
        exception
    );

    /// <summary>
    ///     Creates an error result variant from a message and an optional exception.
    /// </summary>
    /// <remarks>
    ///     This overload dynamically generates the error code using the name of the
    ///     calling method / property
    ///     and the line number where this overload is invoked.
    /// </remarks>
    /// <param name="message">The error message to capture inside the result</param>
    /// <param name="exception">Optional. The exception to capture inside the result</param>
    /// <param name="caller">The caller member name</param>
    /// <param name="lineNumber">The caller line number</param>
    /// <returns>The created result</returns>
    public static ResultType<Empty> Error(
        string message,
        Exception? exception = default,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int lineNumber = 0
    ) => CreateError(
        CreateCode(caller, lineNumber),
        message,
        exception
    );

    private static Error CreateError(
        ErrorCode errorCode,
        string message,
        Exception? exception
    ) => new(errorCode, message, exception);

    private static ErrorCode CreateCode(string caller, int lineNumber)
    {
        if (caller.Length == 0)
        {
            return new ErrorCode($"ERR_{lineNumber:D4}");
        }

        IEnumerable<char> upperCaseChars = caller.Where(char.IsUpper);
        string upperCaseCharsString = string.Join("", upperCaseChars);

        return upperCaseCharsString.Length switch
        {
            > 3 => new ErrorCode(
                $"{upperCaseCharsString[..3]}_{lineNumber:D4}"
                   .ToUpperInvariant()
            ),
            < 3 => new ErrorCode(
                $"{caller[..3]}_{lineNumber:D4}".ToUpperInvariant()
            ),
            var _ => new ErrorCode($"{upperCaseCharsString}_{lineNumber:D4}"),
        };
    }
}
