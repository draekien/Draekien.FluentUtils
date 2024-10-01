namespace FluentUtils.Monad;

/// <summary>
///     The <see cref="ResultType{T}" /> variant that represents an error and
///     containing an <see cref="Error" /> value
/// </summary>
/// <remarks>
///     Do not access this record directly - invoke the <c>Unwrap</c> or
///     <c>Match</c> extension methods instead
/// </remarks>
/// <typeparam name="T">
///     The value type associated with it's
///     <see cref="OkResultType{T}" /> counterpart
/// </typeparam>
public sealed record ErrorResultType<T> : ResultType<T>
{
    internal ErrorResultType(Error error, ILogger? logger = null)
    {
        Error = error;
        if (logger is not null) Logger = logger;
    }

    /// <summary>
    ///     The <see cref="Error" />
    /// </summary>
    public Error Error { get; }
}
