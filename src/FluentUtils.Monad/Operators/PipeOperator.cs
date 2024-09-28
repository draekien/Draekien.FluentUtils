namespace FluentUtils.Monad.Operators;

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

[PublicAPI]
public static class PipeOperator
{
    [DebuggerStepperBoundary]
    public static ResultType<TOut> Pipe<TIn, TOut>(
        this ResultType<TIn> result,
        Func<TIn, TOut> pipe,
        [CallerArgumentExpression(nameof(pipe))]
        string pipeExpression = "")
    {
        return result.Match(
            value =>
            {
                string outputType = typeof(TOut).Name;
                try
                {
                    result.Logger.LogDebug(
                        "Piping result from {ValueType} to {OutputType}",
                        result.ValueType.Name,
                        outputType);

                    TOut output = pipe(value);

                    result.Logger.LogDebug(
                        "Successfully piped result from {ValueType} to {OutputType}",
                        result.ValueType.Name,
                        outputType);

                    return output;
                }
                catch (Exception ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "Failed to pipe result from {ValueType} to {OutputType}",
                        result.ValueType.Name,
                        outputType);

                    return MonadErrors.FailedToPipeValue(ex, pipeExpression);
                }
            },
            Result.Error<TOut>);
    }

    [DebuggerStepperBoundary]
    public static async Task<ResultType<TOut>> Pipe<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, Task<TOut>> pipe,
        [CallerArgumentExpression(nameof(pipe))]
        string pipeExpression = "")
    {
        ResultType<TIn> result = await resultTask;

        return await result.Match(
            async value =>
            {
                string outputType = typeof(TOut).Name;
                try
                {
                    result.Logger.LogDebug(
                        "Piping result from {ValueType} to {OutputType}",
                        result.ValueType.Name,
                        outputType);

                    TOut output = await pipe(value);

                    result.Logger.LogDebug(
                        "Successfully piped result from {ValueType} to {OutputType}",
                        result.ValueType.Name,
                        outputType);

                    return output;
                }
                catch (Exception ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "Failed to pipe result from {ValueType} to {OutputType}",
                        result.ValueType.Name,
                        outputType);

                    return MonadErrors.FailedToPipeValue(ex, pipeExpression);
                }
            },
            Result.ErrorAsync<TOut>);
    }
}
