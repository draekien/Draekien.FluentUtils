namespace FluentUtils.Monad.Exceptions;

/// <summary>
///     An exception that occurs for an unexpected <see cref="ResultType{T}" />
/// </summary>
/// <param name="result">The <see cref="ResultType{T}" /></param>
/// <typeparam name="T">The result value's type</typeparam>
[PublicAPI]
public sealed class UnsupportedResultTypeException<T>(ResultType<T> result)
    : NotSupportedException(
        $"The result type '{result.GetType().Name}' is not supported."
    );
