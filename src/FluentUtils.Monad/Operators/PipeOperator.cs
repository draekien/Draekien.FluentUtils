namespace FluentUtils.Monad.Operators;

/// <summary>
///     Transform the value of a result
/// </summary>
[PublicAPI]
public static class PipeOperator
{
    /// <summary>
    ///     Pipe the value of a <see cref="ResultType{T}" /> into a different type
    /// </summary>
    /// <remarks>
    ///     The pipe operator will only be executed if the input
    ///     <see cref="ResultType{T}" /> is an <see cref="OkResultType{T}" />
    /// </remarks>
    /// <param name="result">The <see cref="ResultType{T}" /></param>
    /// <param name="pipe">A method that transforms the value to another type</param>
    /// <param name="pipeExpression">The pipe expression string</param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <typeparam name="TOut">The output result value's type</typeparam>
    /// <returns>A <see cref="ResultType{T}" /> with the transformed value</returns>
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

                    return Result.Ok(output, result.Logger);
                }
                catch (UnwrapPanicException ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "Failed to unwrap when piping result from {ValueType} to {OutputType}",
                        result.ValueType.Name,
                        outputType);

                    return Result.Error<TOut>(ex.Error, result.Logger);
                }
                catch (Exception ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "Failed to pipe result from {ValueType} to {OutputType}",
                        result.ValueType.Name,
                        outputType);

                    return Result.Error<TOut>(
                        MonadErrors.FailedToPipeValue(ex, pipeExpression),
                        result.Logger);
                }
            },
            error => Result.Error<TOut>(error, result.Logger));
    }

    /// <summary>
    ///     Pipe the value of a <see cref="ResultType{T}" /> into a different type
    /// </summary>
    /// <remarks>
    ///     The pipe operator will only be executed if the input
    ///     <see cref="ResultType{T}" /> is an <see cref="OkResultType{T}" />
    /// </remarks>
    /// <param name="result">The <see cref="ResultType{T}" /></param>
    /// <param name="pipe">A method that transforms the value to another type</param>
    /// <param name="pipeExpression">The pipe expression string</param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <typeparam name="TOut">The output result value's type</typeparam>
    /// <returns>A <see cref="ResultType{T}" /> with the transformed value</returns>
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

                    return await Result.OkAsync(output, result.Logger);
                }
                catch (Exception ex)
                {
                    result.Logger.LogWarning(
                        ex,
                        "Failed to pipe result from {ValueType} to {OutputType}",
                        result.ValueType.Name,
                        outputType);

                    return await Result.ErrorAsync<TOut>(
                        MonadErrors.FailedToPipeValue(ex, pipeExpression),
                        result.Logger);
                }
            },
            error => Result.ErrorAsync<TOut>(error, result.Logger));
    }

    /// <summary>
    ///     Pipe the value of a <see cref="ResultType{T}" /> into a different type
    /// </summary>
    /// <remarks>
    ///     The pipe operator will only be executed if the input
    ///     <see cref="ResultType{T}" /> is an <see cref="OkResultType{T}" />
    /// </remarks>
    /// <param name="resultTask">The <see cref="ResultType{T}" /></param>
    /// <param name="pipe">A method that transforms the value to another type</param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <typeparam name="TOut">The output result value's type</typeparam>
    /// <returns>A <see cref="ResultType{T}" /> with the transformed value</returns>
    public static async Task<ResultType<TOut>> Pipe<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, Task<TOut>> pipe)
    {
        ResultType<TIn> result = await resultTask;

        return await result.Pipe(pipe);
    }

    /// <summary>
    ///     Pipe the value of a <see cref="ResultType{T}" /> into a different type
    /// </summary>
    /// <remarks>
    ///     The pipe operator will only be executed if the input
    ///     <see cref="ResultType{T}" /> is an <see cref="OkResultType{T}" />
    /// </remarks>
    /// <param name="resultTask">The <see cref="ResultType{T}" /></param>
    /// <param name="pipe">A method that transforms the value to another type</param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <typeparam name="TOut">The output result value's type</typeparam>
    /// <returns>A <see cref="ResultType{T}" /> with the transformed value</returns>
    public static async Task<ResultType<TOut>> Pipe<TIn, TOut>(
        this Task<ResultType<TIn>> resultTask,
        Func<TIn, TOut> pipe)
    {
        ResultType<TIn> result = await resultTask;

        return result.Pipe(pipe);
    }
}
