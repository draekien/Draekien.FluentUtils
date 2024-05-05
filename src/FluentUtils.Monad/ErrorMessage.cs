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
}