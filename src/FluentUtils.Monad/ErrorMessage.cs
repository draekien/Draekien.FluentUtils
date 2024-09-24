namespace FluentUtils.Monad;

/// <summary>
///     A message that is associated with a runtime <see cref="Error" />
/// </summary>
/// <param name="Value">The message</param>
[PublicAPI]
public readonly record struct ErrorMessage(string Value)
{
    /// <summary>
    ///     Returns the value of the <see cref="ErrorMessage" />
    /// </summary>
    /// <returns>The value of the <see cref="ErrorMessage" /></returns>
    public override string ToString() => Value;

    /// <summary>
    ///     Implicitly creates a new instance of <see cref="ErrorMessage" /> from a
    ///     string value
    /// </summary>
    /// <param name="value">The error message string</param>
    /// <returns>The <see cref="ErrorMessage" /> instance</returns>
    public static implicit operator ErrorMessage(string value) => new(value);

    /// <summary>
    ///     Implicitly converts an <see cref="ErrorMessage" /> to a string.
    /// </summary>
    /// <param name="value">The error message.</param>
    /// <returns>The string.</returns>
    public static implicit operator string(ErrorMessage value) => value.Value;
}
