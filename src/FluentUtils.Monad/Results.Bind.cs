namespace FluentUtils.Monad;

using System.Runtime.CompilerServices;

public static partial class Results
{
    public static ResultType<T> Bind<T>(
        Func<T> factory,
        [CallerArgumentExpression(nameof(factory))]
        string factoryExpression = "")
    {
        try
        {
            T output = factory();
            return Ok(output);
        }
        catch (Exception ex)
        {
            Error error =
                MonadErrors.FailedToBindFactory(ex, factoryExpression);
            return Error<T>(error);
        }
    }

    public static async Task<ResultType<T>> BindAsync<T>(
        Func<Task<T>> asyncFactory,
        [CallerArgumentExpression(nameof(asyncFactory))]
        string factoryExpression = "")
    {
        try
        {
            T output = await asyncFactory();
            return await OkAsync(output);
        }
        catch (Exception ex)
        {
            Error error =
                MonadErrors.FailedToBindFactory(ex, factoryExpression);
            return await ErrorAsync<T>(error);
        }
    }

    public static async Task<ResultType<T>> BindAsync<T>(
        Func<CancellationToken, Task<T>> asyncFactory,
        CancellationToken cancellation = default,
        [CallerArgumentExpression(nameof(asyncFactory))]
        string factoryExpression = "")
    {
        try
        {
            T output = await asyncFactory(cancellation);
            return await OkAsync(output);
        }
        catch (Exception ex)
        {
            Error error =
                MonadErrors.FailedToBindFactory(ex, factoryExpression);
            return await ErrorAsync<T>(error);
        }
    }
}
