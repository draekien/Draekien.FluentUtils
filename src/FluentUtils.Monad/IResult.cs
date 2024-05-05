using JetBrains.Annotations;

namespace FluentUtils.Monad;

/// <summary>
///     The result monad
/// </summary>
/// <typeparam name="T">The type of the value associated with the result</typeparam>
[PublicAPI]
public interface IResult<T> where T : notnull;