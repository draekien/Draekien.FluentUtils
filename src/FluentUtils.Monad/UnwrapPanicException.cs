using JetBrains.Annotations;

namespace FluentUtils.Monad;

/// <summary>
///     A <see cref="InvalidOperationException" /> which occurs when unwrapping an errored <see cref="IResult{T}" />
/// </summary>
/// <param name="error">The <see cref="Error" /> stored in the errored <see cref="IResult{T}" /></param>
[PublicAPI]
public sealed class UnwrapPanicException(Error error)
    : InvalidOperationException(error.ToString(), error.Exception);