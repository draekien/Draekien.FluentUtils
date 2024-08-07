﻿namespace FluentUtils.Monad.Extensions;

using System.Runtime.CompilerServices;

/// <summary>
///     Extensions for mapping the value of a <see cref="ResultType{T}" /> to a
///     different type
/// </summary>
[PublicAPI]
public static class PipeExtensions
{
    /// <summary>
    ///     Pipes the value of an synchronous <see cref="ResultType{T}" /> to another
    ///     type if the <see cref="ResultType{T}" />
    ///     is an <see cref="OkResultType{T}" />, otherwise pipes the
    ///     <see cref="ErrorResultType{T}" /> to the target type
    /// </summary>
    /// <param name="result">The <see cref="ResultType{T}" /></param>
    /// <param name="pipe">The pipe operation</param>
    /// <param name="pipeExpression">The pipe expression</param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <typeparam name="TOut">The output result value's type</typeparam>
    /// <returns>
    ///     A <see cref="ResultType{T}" /> containing the piped value, or the
    ///     forwarded <see cref="ErrorResultType{T}" />
    /// </returns>
    public static ResultType<TOut> Pipe<TIn, TOut>(
        this ResultType<TIn> result,
        Func<TIn, TOut> pipe,
        [CallerArgumentExpression(nameof(pipe))]
        string pipeExpression = ""
    ) =>
        result.Match(
            value =>
            {
                try
                {
                    TOut transformed = pipe(value);
                    return Result.Ok(transformed);
                }
                catch (Exception ex)
                {
                    return MonadErrors.FailedToPipeValue(
                        ex,
                        pipeExpression);
                }
            },
            Result.Error<TOut>);
}
