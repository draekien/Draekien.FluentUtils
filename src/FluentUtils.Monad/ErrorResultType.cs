using JetBrains.Annotations;

namespace FluentUtils.Monad;

/// <summary>
///     An <see cref="ErrorResultType{T}"/> for a result which would have returned an <see cref="Empty"/> value
/// </summary>
/// <param name="Error">The <see cref="Error"/></param>
public record ErrorResultType(Error Error) : ErrorResultType<Empty>(Error);

/// <summary>
///     The <see cref="ResultType{T}" /> variant that represents an error and containing an <see cref="Error" /> value
/// </summary>
/// <remarks>
///     Do not access this record directly - invoke the <c>Unwrap</c> or <c>Match</c> extension methods instead
/// </remarks>
/// <typeparam name="T">The value type associated with it's <see cref="OkResultType{T}" /> counterpart</typeparam>
[PublicAPI]
public record ErrorResultType<T> : ResultType<T> where T : notnull
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