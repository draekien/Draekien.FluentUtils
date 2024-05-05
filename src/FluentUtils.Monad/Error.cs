using JetBrains.Annotations;

namespace FluentUtils.Monad;

/// <summary>
///     A record that represents a runtime error
/// </summary>
/// <param name="Code">The <see cref="ErrorCode" /></param>
/// <param name="Message">The <see cref="ErrorMessage" /></param>
/// <param name="Exception">The <see cref="Exception" /> that caused the error</param>
[PublicAPI]
public record Error(ErrorCode Code, ErrorMessage Message, Exception? Exception = default)
{
    /// <summary>
    ///     Returns the string representation of the <see cref="Error" />
    /// </summary>
    /// <returns>The string representation of the <see cref="Error" /></returns>
    public override string ToString()
    {
        return $"{Code}: {Message}";
    }
}