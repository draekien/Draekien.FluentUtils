namespace FluentUtils.Monad;

/// <summary>
///     The <see cref="ResultType{T}" /> variant that represents a success and
///     contains a value
/// </summary>
/// <remarks>
///     Do not access this record directly - invoke the <c>Unwrap</c> or
///     <c>Match</c> extension methods instead
/// </remarks>
/// <typeparam name="T">The value type</typeparam>
public sealed record OkResultType<T> : ResultType<T>
{
    internal OkResultType(T value, ILogger? logger = null)
    {
        Value = value;
        if (logger is not null) Logger = logger;
    }

    internal T Value { get; }
}
