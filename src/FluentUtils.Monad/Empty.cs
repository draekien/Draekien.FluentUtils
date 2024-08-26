namespace FluentUtils.Monad;

/// <summary>
///     A struct that represents an empty value. It is used when a method returns a
///     <see cref="ResultType{T}" /> that
///     contains
///     no value.
/// </summary>
[PublicAPI]
public readonly record struct Empty
{
    /// <summary>
    ///     The default <see cref="Empty" /> value
    /// </summary>
    public static readonly Empty Default;
}
