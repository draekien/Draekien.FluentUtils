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
    internal ErrorResultType(Error error)
    {
        Error = error;
    }

    /// <summary>
    ///     The <see cref="Error" />
    /// </summary>
    public Error Error { get; }

    /// <summary>
    ///     Converts a <see cref="ErrorResultType{T}" /> of type <see cref="T" /> to
    ///     a type of <see cref="TOut" />, preserving the error inside the result
    /// </summary>
    /// <typeparam name="TOut">The output type</typeparam>
    /// <returns>A <see cref="ErrorResultType{T}" /> where T is the output type</returns>
    public ResultType<TOut> To<TOut>() where TOut : notnull =>
        new ErrorResultType<TOut>(Error);
}
