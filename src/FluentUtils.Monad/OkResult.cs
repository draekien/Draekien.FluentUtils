namespace FluentUtils.Monad;

/// <summary>
///     The <see cref="IResult{T}" /> variant that represents a success and contains a value
/// </summary>
/// <remarks>
///     Do not access this record directly - invoke the <c>Unwrap</c> or <c>Match</c> extension methods instead
/// </remarks>
/// <typeparam name="T">The value type</typeparam>
public sealed record OkResult<T> : IResult<T> where T : notnull
{
    /// <summary>
    /// Do not use directly
    /// </summary>
    /// <param name="value"></param>
    internal OkResult(T value)
    {
        Value = value;
    }

    internal T Value { get; init; }

    internal void Deconstruct(out T value)
    {
        value = Value;
    }
}