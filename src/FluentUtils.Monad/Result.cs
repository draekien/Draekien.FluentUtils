using JetBrains.Annotations;

namespace FluentUtils.Monad;

/// <summary>
///     A result is the type used for returning and propagating errors. It is a monad with the variants
///     <see cref="OkResult{T}" />, representing success and containing some value, and <see cref="ErrorResult{T}" />,
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
    public static IResult<T> Ok<T>(T value) where T : notnull
    {
        return new OkResult<T>(value);
    }

    /// <summary>
    ///     Creates a successful result variant which holds no value
    /// </summary>
    /// <returns>The created result</returns>
    public static IResult<Empty> Ok()
    {
        return Ok<Empty>(default);
    }

    /// <summary>
    ///     Creates an error result variant
    /// </summary>
    /// <param name="error">The error value</param>
    /// <typeparam name="T">The value type associated with its success result counterpart</typeparam>
    /// <returns>The created result</returns>
    public static IResult<T> Error<T>(Error error) where T : notnull
    {
        return new ErrorResult<T>(error);
    }

    /// <summary>
    ///     Creates an error result variant which is the counterpart to a success result that holds no value
    /// </summary>
    /// <param name="error">The error value</param>
    /// <returns>The created result</returns>
    public static IResult<Empty> Error(Error error)
    {
        return Error<Empty>(error);
    }
}