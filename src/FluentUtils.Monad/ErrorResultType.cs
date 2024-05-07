namespace FluentUtils.Monad;

/// <summary>
///     The <see cref="ResultType{T}" /> variant that represents an error and containing an <see cref="Error" /> value
/// </summary>
/// <remarks>
///     Do not access this record directly - invoke the <c>Unwrap</c> or <c>Match</c> extension methods instead
/// </remarks>
/// <typeparam name="T">The value type associated with it's <see cref="OkResultType{T}" /> counterpart</typeparam>
public sealed record ErrorResultType<T> : ResultType<T> where T : notnull
{
    internal ErrorResultType(Error error)
    {
        Error = error;
    }

    internal Error Error { get; init; }

    internal void Deconstruct(out Error error)
    {
        error = Error;
    }
}