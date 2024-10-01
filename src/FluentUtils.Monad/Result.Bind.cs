namespace FluentUtils.Monad;

public static partial class Result
{
    /// <summary>
    ///     Binds the returned value of a factory method to a
    ///     <see cref="ResultType{T}" />.
    /// </summary>
    /// <param name="factory">The factory method to invoke.</param>
    /// <param name="logger">An optional <see cref="ILogger" /> instance</param>
    /// <param name="factoryExpression">
    ///     The string representation of the factory
    ///     method.
    /// </param>
    /// <typeparam name="T">The return type of the factory.</typeparam>
    /// <returns>
    ///     A <see cref="OkResultType{T}" /> when the factory method does not
    ///     throw an exception, otherwise an <see cref="ErrorResultType{T}" />.
    /// </returns>
    public static ResultType<T> Bind<T>(
        Func<T> factory,
        ILogger? logger = default,
        [CallerArgumentExpression(nameof(factory))]
        string factoryExpression = "")
    {
        try
        {
            T output = factory();
            return Ok(output, logger);
        }
        catch (Exception ex)
        {
            Error error =
                MonadErrors.FailedToBindFactory(ex, factoryExpression);
            return Error<T>(error, logger);
        }
    }

    /// <summary>
    ///     Binds the returned value of an asynchronous factory method to a
    ///     <see cref="ResultType{T}" />.
    /// </summary>
    /// <param name="asyncFactory">The asynchronous factory method to invoke.</param>
    /// <param name="logger">An optional <see cref="ILogger" /> instance</param>
    /// <param name="factoryExpression">
    ///     The string representation of the factory
    ///     method.
    /// </param>
    /// <typeparam name="T">The return type of the factory.</typeparam>
    /// <returns>
    ///     A task which when awaited returns an <see cref="OkResultType{T}" /> when
    ///     the factory method does not
    ///     throw an exception, otherwise an <see cref="ErrorResultType{T}" />.
    /// </returns>
    public static async Task<ResultType<T>> Bind<T>(
        Func<Task<T>> asyncFactory,
        ILogger? logger = default,
        [CallerArgumentExpression(nameof(asyncFactory))]
        string factoryExpression = "")
    {
        try
        {
            T output = await asyncFactory();
            return await OkAsync(output, logger);
        }
        catch (Exception ex)
        {
            Error error =
                MonadErrors.FailedToBindFactory(ex, factoryExpression);
            return await ErrorAsync<T>(error, logger);
        }
    }
}
