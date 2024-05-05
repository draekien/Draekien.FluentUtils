using JetBrains.Annotations;

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
    public override string ToString()
    {
        return Value;
    }
}