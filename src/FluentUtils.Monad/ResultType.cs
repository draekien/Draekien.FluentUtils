using JetBrains.Annotations;

namespace FluentUtils.Monad;

/// <summary>
///     The result monad
/// </summary>
/// <typeparam name="T">The type of the value associated with the result</typeparam>
[PublicAPI]
public abstract record ResultType<T> where T : notnull
{
    /// <summary>
    ///     Implicitly creates an instance of <see cref="OkResultType{T}" /> from some value type T
    /// </summary>
    /// <param name="value">The value to wrap inside a result</param>
    /// <returns>The <see cref="OkResultType{T}" /></returns>
    public static implicit operator ResultType<T>(T value)
    {
        return new OkResultType<T>(value);
    }

    /// <summary>
    ///     Creates a new <see cref="ErrorResultType{T}" /> from an instance of <see cref="Error" />
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static implicit operator ResultType<T>(Error error)
    {
        return new ErrorResultType<T>(error);
    }

    /// <summary>
    ///     Creates a new <see cref="ErrorResultType{T}" /> from an exception, automatically generating an error code and
    ///     extracting the error message from the exception
    /// </summary>
    /// <param name="exception">The exception</param>
    /// <returns>The error result</returns>
    public static implicit operator ResultType<T>(Exception exception)
    {
        var upperCaseLetters = exception.GetType().Name.Where(char.IsUpper).ToArray();

        ErrorCode code = new($"DYN_{string.Concat(upperCaseLetters)}");
        ErrorMessage message = new(exception.Message);
        Error error = new(code, message, exception);

        return new ErrorResultType<T>(error);
    }
}