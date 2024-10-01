namespace FluentUtils.Monad;

public static partial class Result
{
    /// <summary>
    ///     Creates an <see cref="ErrorResultType{T}" /> from an
    ///     <see cref="Monad.Error" />.
    /// </summary>
    /// <param name="error">The <see cref="Monad.Error" />.</param>
    /// <param name="logger">An optional <see cref="ILogger" /> instance</param>
    /// <typeparam name="T">
    ///     The underlying value contained within the
    ///     <see cref="ResultType{T}" />.
    /// </typeparam>
    /// <returns>
    ///     An <see cref="ErrorResultType{T}" /> as a <see cref="ResultType{T}" />.
    /// </returns>
    public static ResultType<T> Error<T>(Error error, ILogger? logger = null) =>
        new ErrorResultType<T>(error, logger);

    /// <summary>
    ///     Creates an <see cref="ErrorResultType{T}" />
    ///     from an <see cref="Monad.Error" />.
    /// </summary>
    /// <param name="error">The <see cref="Monad.Error" />.</param>
    /// <param name="logger">An optional <see cref="ILogger" /> instance</param>
    /// <returns>
    ///     An <see cref="ErrorResultType{T}" /> as a <see cref="ResultType{T}" />
    /// </returns>
    public static ResultType<Empty>
        Error(Error error, ILogger? logger = null) =>
        Error<Empty>(error, logger);

    internal static Task<ResultType<T>> ErrorAsync<T>(
        Error error,
        ILogger? logger = null) =>
        Task.FromResult(Error<T>(error, logger));

    internal static Task<ResultType<Empty>> ErrorAsync(
        Error error,
        ILogger? logger = null) =>
        ErrorAsync<Empty>(error, logger);
}
