using System.Net;
using JetBrains.Annotations;

namespace FluentUtils.Monad;

/// <summary>
///     A subclass of <see cref="Error" /> which is associated with a <see cref="HttpStatusCode" />. Useful for binding
///     errors to response types in a Web API application.
/// </summary>
/// <param name="Code">The <see cref="ErrorCode" /></param>
/// <param name="Message">The <see cref="ErrorMessage" /></param>
/// <param name="HttpStatusCode">
///     The <see cref="HttpStatusCode" />. Defaults to
///     <see cref="System.Net.HttpStatusCode.InternalServerError" />
/// </param>
/// <param name="Exception">The <see cref="Exception" /> that caused the error</param>
[PublicAPI]
public record HttpStatusCodeError(
    ErrorCode Code,
    ErrorMessage Message,
    HttpStatusCode HttpStatusCode = HttpStatusCode.InternalServerError,
    Exception? Exception = default) : Error(Code, Message, Exception);