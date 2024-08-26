namespace FluentUtils.Monad;

public static partial class Results
{
    internal static Task<ResultType<T>> OkAsync<T>(
        T value) =>
        Task.FromResult<ResultType<T>>(new OkResultType<T>(value));

    internal static Task<ResultType<T>> ErrorAsync<T>(Error error) =>
        Task.FromResult<ResultType<T>>(new ErrorResultType<T>(error));
}
