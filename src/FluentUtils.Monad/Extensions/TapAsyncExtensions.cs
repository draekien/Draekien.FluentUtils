namespace FluentUtils.Monad.Extensions;

using System.Runtime.CompilerServices;

public static class TapAsyncExtensions
{
    public static Task<ResultType<TIn>> TapAsync<TIn>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, Task> tapAsync,
        [CallerArgumentExpression(nameof(tapAsync))]
        string tapExpression =
            "") => resultTask.PipeAsync(
        async value =>
        {
            try
            {
                await tapAsync(value);
                return await Result.OkAsync(value);
            }
            catch (Exception ex)
            {
                return await Result.ErrorAsync<TIn>(
                    MonadErrors.FailedToTapValue(ex, tapExpression));
            }
        });

    public static async Task<ResultType<TIn>> TapAsync<TIn>(
        this Task<ResultType<TIn>> resultTask,
        Action<TIn> tap,
        [CallerArgumentExpression(nameof(tap))]
        string tapExpression = "") =>
        await resultTask.PipeAsync(
            async value =>
            {
                try
                {
                    tap(value);
                    return await Result.OkAsync(value);
                }
                catch (Exception ex)
                {
                    return await Result.ErrorAsync<TIn>(
                        MonadErrors.FailedToTapValue(ex, tapExpression));
                }
            });
}
