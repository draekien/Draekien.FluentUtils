namespace FluentUtils.Monad;

/// <summary>
///     A code that is associated with a runtime <see cref="Error" />
/// </summary>
/// <param name="Value">The code</param>
[PublicAPI]
public readonly record struct ErrorCode(string Value)
{
    /// <summary>
    ///     Returns the value of the <see cref="ErrorCode" />
    /// </summary>
    /// <returns>The value of the <see cref="ErrorCode" /></returns>
    public override string ToString() => Value;

    /// <summary>
    ///     Implicitly creates a new instance of <see cref="ErrorCode" /> from a string
    ///     value
    /// </summary>
    /// <param name="value">The error code string</param>
    /// <returns>The <see cref="ErrorCode" /> instance</returns>
    public static implicit operator ErrorCode(string value) => new(value);
}
