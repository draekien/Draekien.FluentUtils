namespace FluentUtils.Monad;

using System.Runtime.CompilerServices;
using JetBrains.Annotations;

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
    ///     Creates a success result variant
    /// </summary>
    /// <param name="value">The success value</param>
    /// <typeparam name="T">The value type</typeparam>
    /// <returns>The created result</returns>
    public static ResultType<T> Ok<T>(T value) where T : notnull =>
        new OkResultType<T>(value);

    /// <summary>
    ///     Creates a successful result variant which holds no value
    /// </summary>
    /// <returns>The created result</returns>
    public static ResultType<Empty> Ok() => Ok<Empty>(default);

    /// <summary>
    ///     Creates an error result variant
    /// </summary>
    /// <param name="error">The error value</param>
    /// <typeparam name="T">
    ///     The value type associated with its success result
    ///     counterpart
    /// </typeparam>
    /// <returns>The created result</returns>
    public static ResultType<T> Error<T>(Error error) where T : notnull =>
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
    ) where T : notnull => CreateError(
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
