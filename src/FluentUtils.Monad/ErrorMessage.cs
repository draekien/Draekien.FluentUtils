using JetBrains.Annotations;

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
    public override string ToString()
    {
        return Value;
    }

    /// <summary>
    ///     Implicitly creates a new instance of <see cref="ErrorMessage" /> from a string value
    /// </summary>
    /// <param name="value">The error message string</param>
    /// <returns>The <see cref="ErrorMessage" /> instance</returns>
    public static implicit operator ErrorMessage(string value)
    {
        return new ErrorMessage(value);
    }
}