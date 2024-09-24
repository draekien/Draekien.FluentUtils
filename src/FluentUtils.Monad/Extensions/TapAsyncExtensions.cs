namespace FluentUtils.Monad.Extensions;

using System.Runtime.CompilerServices;

public static class TapAsyncExtensions
{
    public static Task<ResultType<TIn>> TapAsync<TIn>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, Task> tapAsync,
        [CallerArgumentExpression(nameof(tapAsync))]
        string tapExpression =
            "") => resultTask.MatchAsync(
        async value =>
        {
            try
            {
                await tapAsync(value);
                return await resultTask;
            }
            catch (Exception ex)
            {
                return await Result.ErrorAsync<TIn>(
                    MonadErrors.FailedToTapValue(ex, tapExpression));
            }
        },
        Result.ErrorAsync<TIn>);
}
