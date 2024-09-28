namespace FluentUtils.Monad;

public static partial class Result
{
    /// <summary>
    ///     Creates an <see cref="OkResultType{T}" /> from the provided value.
    /// </summary>
    /// <param name="value">
    ///     The underlying value of the <see cref="ResultType{T}" />
    /// </param>
    /// <typeparam name="T">The value type</typeparam>
    /// <returns>An <see cref="OkResultType{T}" /> as a <see cref="ResultType{T}" />.</returns>
    public static ResultType<T> Ok<T>(T value) => new OkResultType<T>(value);

    public static async Task<ResultType<T>> Ok<T>(Task<T> value) =>
        await OkAsync(await value);

    /// <summary>
    ///     Creates an <see cref="OkResultType{T}" /> with an empty value.
    /// </summary>
    /// <returns>
    ///     An <see cref="OkResultType{T}" /> as a <see cref="ResultType{T}" />
    /// </returns>
    public static ResultType<Empty> Ok() => Ok(Empty.Default);

    internal static Task<ResultType<T>> OkAsync<T>(
        T value) =>
        Task.FromResult(Ok(value));

    internal static Task<ResultType<Empty>> OkAsync() => OkAsync(Empty.Default);
}
