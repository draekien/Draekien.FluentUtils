namespace FluentUtils.Monad.Operators;

[PublicAPI]
public static class PipeOperator
{
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
                catch (UnwrapPanicException ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "Failed to unwrap when piping result from {ValueType} to {OutputType}",
                        result.ValueType.Name,
                        outputType);

                    return ex.Error;
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

    public static async Task<ResultType<TOut>> Pipe<TIn, TOut>(
        this ResultType<TIn> result,
        Func<TIn, Task<TOut>> pipe,
        [CallerArgumentExpression(nameof(pipe))]
        string pipeExpression = "")
    {
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

    public static async Task<ResultType<TOut>> Pipe<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, Task<TOut>> pipe)
    {
        ResultType<TIn> result = await resultTask;

        return await result.Pipe(pipe);
    }

    public static async Task<ResultType<TOut>> Pipe<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, TOut> pipe)
    {
        ResultType<TIn> result = await resultTask;

        return result.Pipe(pipe);
    }

    // [DebuggerStepperBoundary]
    // public static ResultType<TOut> Pipe<TIn, TOut>(
    //     this ResultType<TIn> result,
    //     Func<TIn, TOut> pipe,
    //     [CallerArgumentExpression(nameof(pipe))]
    //     string pipeExpression = "")
    // {
    //     return result.Match(
    //         value =>
    //         {
    //             string outputType = typeof(TOut).Name;
    //             try
    //             {
    //                 result.Logger.LogDebug(
    //                     "Piping result from {ValueType} to {OutputType}",
    //                     result.ValueType.Name,
    //                     outputType);
    //
    //                 TOut output = pipe(value);
    //
    //                 result.Logger.LogDebug(
    //                     "Successfully piped result from {ValueType} to {OutputType}",
    //                     result.ValueType.Name,
    //                     outputType);
    //
    //                 return output;
    //             }
    //             catch (Exception ex)
    //             {
    //                 result.Logger.LogWarning(
    //                     ex,
    //                     "Failed to pipe result from {ValueType} to {OutputType}",
    //                     result.ValueType.Name,
    //                     outputType);
    //
    //                 return MonadErrors.FailedToPipeValue(ex, pipeExpression);
    //             }
    //         },
    //         Result.Error<TOut>);
    // }
    //
    // [DebuggerStepperBoundary]
    // public static async Task<ResultType<TOut>> Pipe<TIn, TOut>(
    //     this Task<ResultType<TIn>> resultTask,
    //     Func<TIn, Task<TOut>> pipe,
    //     [CallerArgumentExpression(nameof(pipe))]
    //     string pipeExpression = "")
    // {
    //     ResultType<TIn> result = await resultTask;
    //
    //     return await result.Match(
    //         async value =>
    //         {
    //             string outputType = typeof(TOut).Name;
    //             try
    //             {
    //                 result.Logger.LogDebug(
    //                     "Piping result from {ValueType} to {OutputType}",
    //                     result.ValueType.Name,
    //                     outputType);
    //
    //                 TOut output = await pipe(value);
    //
    //                 result.Logger.LogDebug(
    //                     "Successfully piped result from {ValueType} to {OutputType}",
    //                     result.ValueType.Name,
    //                     outputType);
    //
    //                 return output;
    //             }
    //             catch (Exception ex)
    //             {
    //                 result.Logger.LogWarning(
    //                     ex,
    //                     "Failed to pipe result from {ValueType} to {OutputType}",
    //                     result.ValueType.Name,
    //                     outputType);
    //
    //                 return MonadErrors.FailedToPipeValue(ex, pipeExpression);
    //             }
    //         },
    //         Result.ErrorAsync<TOut>);
    // }
    //
    // public static ResultType<TOut> Pipe<TIn, TOut>(
    //     this ResultType<TIn> result,
    //     Func<TIn, ResultType<TOut>> pipe)
    // {
    //     return result.Pipe(value => pipe.Invoke(value).Unwrap());
    // }
    //
    // public static async Task<ResultType<TOut>> Pipe<TIn, TOut>(
    //     this Task<ResultType<Task<TIn>>> resultTask,
    //     Func<TIn, TOut> pipe)
    // {
    //     ResultType<Task<TIn>> result = await resultTask;
    //     return await result.Match(
    //         async valueTask =>
    //         {
    //             TIn value = await valueTask;
    //             TOut output = pipe(value);
    //             return await Result.OkAsync(output);
    //         },
    //         Result.ErrorAsync<TOut>);
    // }
    //
    // public static async Task<ResultType<TOut>> Pipe<TIn, TOut>(
    //     this Task<ResultType<TIn>> resultTask,
    //     Func<TIn, TOut> pipe) =>
    //     (await resultTask).Pipe(pipe);
    //
    // public static async Task<ResultType<TOut>> Pipe<TIn, TOut>(
    //     this Task<ResultType<TIn>> resultTask,
    //     Func<TIn, ResultType<TOut>> pipe) =>
    //     (await resultTask).Pipe(pipe);
}
