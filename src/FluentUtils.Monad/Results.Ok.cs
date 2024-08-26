namespace FluentUtils.Monad;

public static partial class Results
{
    /// <summary>
    ///     Creates a new <see cref="ResultType{T}" /> of subclass
    ///     <see cref="OkResultType{T}" />
    /// </summary>
    /// <param name="value">The value of the <see cref="OkResultType{T}" /></param>
    /// <typeparam name="T">The value type</typeparam>
    /// <returns>The <see cref="ResultType{T}" /></returns>
    public static ResultType<T> Ok<T>(T value) => new OkResultType<T>(value);

    public static ResultType<Empty> Ok() => Ok(Empty.Default);
}
