namespace FluentUtils.Monad.Exceptions;

/// <summary>
///     A <see cref="InvalidOperationException" /> which occurs when unwrapping an
///     errored
///     <see cref="FluentUtils.Monad.OkResultType{T}" />
/// </summary>
/// <param name="error">
///     The <see cref="Error" /> stored in the errored
///     <see cref="FluentUtils.Monad.OkResultType{T}" />
/// </param>
[PublicAPI]
public sealed class UnwrapPanicException(Error error)
    : InvalidOperationException(error.ToString(), error.Exception);
