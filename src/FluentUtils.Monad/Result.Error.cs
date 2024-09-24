namespace FluentUtils.Monad;

public static partial class Result
{
    /// <summary>
    ///     Creates an <see cref="ErrorResultType{T}" /> from an
    ///     <see cref="Monad.Error" />.
    /// </summary>
    /// <param name="error">The <see cref="Monad.Error" />.</param>
    /// <typeparam name="T">
    ///     The underlying value contained within the
    ///     <see cref="ResultType{T}" />.
    /// </typeparam>
    /// <returns>
    ///     An <see cref="ErrorResultType{T}" /> as a <see cref="ResultType{T}" />.
    /// </returns>
    public static ResultType<T> Error<T>(Error error) =>
        new ErrorResultType<T>(error);

    /// <summary>
    ///     Creates an <see cref="ErrorResultType{T}" />
    ///     from an <see cref="Monad.Error" />.
    /// </summary>
    /// <param name="error">The <see cref="Monad.Error" />.</param>
    /// <returns>
    ///     An <see cref="ErrorResultType{T}" /> as a <see cref="ResultType{T}" />
    ///     .
    /// </returns>
    public static ResultType<Empty> Error(Error error) => Error<Empty>(error);

    internal static Task<ResultType<T>> ErrorAsync<T>(Error error) =>
        Task.FromResult(Error<T>(error));

    internal static Task<ResultType<Empty>> ErrorAsync(Error error) =>
        ErrorAsync<Empty>(error);
}
