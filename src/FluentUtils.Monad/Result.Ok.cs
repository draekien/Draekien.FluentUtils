namespace FluentUtils.Monad;

public static partial class Result
{
    /// <summary>
    ///     Creates an <see cref="OkResultType{T}" /> from the provided value.
    /// </summary>
    /// <param name="value">
    ///     The underlying value of the <see cref="ResultType{T}" />
    /// </param>
    /// <param name="logger">An optional <see cref="ILogger" /> instance</param>
    /// <typeparam name="T">The value type</typeparam>
    /// <returns>An <see cref="OkResultType{T}" /> as a <see cref="ResultType{T}" />.</returns>
    public static ResultType<T> Ok<T>(T value, ILogger? logger = default) =>
        new OkResultType<T>(value, logger);

    /// <summary>
    ///     Creates an <see cref="OkResultType{T}" /> from the provided value.
    /// </summary>
    /// <param name="value">
    ///     The underlying value of the <see cref="ResultType{T}" />
    /// </param>
    /// <param name="logger">An optional <see cref="ILogger" /> instance</param>
    /// <typeparam name="T">The value type</typeparam>
    /// <returns>An <see cref="OkResultType{T}" /> as a <see cref="ResultType{T}" />.</returns>
    public static async Task<ResultType<T>> Ok<T>(
        Task<T> value,
        ILogger? logger = default) =>
        await OkAsync(await value, logger);

    /// <summary>
    ///     Creates an <see cref="OkResultType{T}" /> with an empty value.
    /// </summary>
    /// <param name="logger">An optional <see cref="ILogger" /> instance</param>
    /// <returns>
    ///     An <see cref="OkResultType{T}" /> as a <see cref="ResultType{T}" />
    /// </returns>
    public static ResultType<Empty> Ok(ILogger? logger = default) =>
        Ok(Empty.Default, logger);

    internal static Task<ResultType<T>> OkAsync<T>(
        T value,
        ILogger? logger = default) =>
        Task.FromResult(Ok(value, logger));

    internal static Task<ResultType<Empty>>
        OkAsync(ILogger? logger = default) => OkAsync(Empty.Default, logger);
}
