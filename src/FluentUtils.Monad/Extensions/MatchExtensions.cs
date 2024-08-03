namespace FluentUtils.Monad.Extensions;

/// <summary>
///     Extensions for matching on a <see cref="ResultType{T}" />
/// </summary>
[PublicAPI]
public static class MatchExtensions
{
    /// <summary>
    ///     Performs a match on the <see cref="ResultType{T}" />, invoking the
    ///     <see cref="okHandler" /> for an <see cref="OkResultType{T}" />
    ///     and the <see cref="errorHandler" /> for an
    ///     <see cref="ErrorResultType{T}" />
    /// </summary>
    /// <param name="result">The <see cref="ResultType{T}" /></param>
    /// <param name="okHandler">
    ///     The operation to perform for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The operation to perform for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <typeparam name="TOut">The output value's type</typeparam>
    /// <returns>The output value from the invoked handler</returns>
    /// <exception cref="UnsupportedResultTypeException{TIn}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static TOut Match<TIn, TOut>(
        this ResultType<TIn> result,
        Func<TIn, TOut> okHandler,
        Func<Error, TOut> errorHandler
    )
    {
        return result switch
        {
            OkResultType<TIn> ok => okHandler(ok.Value),
            ErrorResultType<TIn> err => errorHandler(err.Error),
            var _ => throw new UnsupportedResultTypeException<TIn>(result),
        };
    }

    /// <summary>
    ///     Performs a match on the <see cref="ResultType{T}" />, invoking the
    ///     <see cref="okHandler" /> for an <see cref="OkResultType{T}" />
    ///     and the <see cref="errorHandler" /> for an
    ///     <see cref="ErrorResultType{T}" />
    /// </summary>
    /// <param name="result">The <see cref="ResultType{T}" /></param>
    /// <param name="okHandler">
    ///     The operation to perform for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The operation to perform for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <typeparam name="TOut">The output value's type</typeparam>
    /// <returns>The output value from the invoked handler</returns>
    /// <exception cref="UnsupportedResultTypeException{TIn}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static TOut Match<TOut>(
        this ResultType<Empty> result,
        Func<TOut> okHandler,
        Func<Error, TOut> errorHandler
    ) => result.Match<Empty, TOut>(_ => okHandler(), errorHandler);

    /// <summary>
    ///     Performs a match on the <see cref="ResultType{T}" />, invoking the
    ///     <see cref="okHandler" /> for an <see cref="OkResultType{T}" />
    ///     and the <see cref="errorHandler" /> for an
    ///     <see cref="ErrorResultType{T}" />
    /// </summary>
    /// <param name="result">The <see cref="ResultType{T}" /></param>
    /// <param name="okHandler">
    ///     The operation to perform for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The operation to perform for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <typeparam name="TIn">The input result value's type</typeparam>
    /// <exception cref="UnsupportedResultTypeException{TIn}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static void Match<TIn>(
        this ResultType<TIn> result,
        Action<TIn> okHandler,
        Action<Error> errorHandler
    )
    {
        result.Match<TIn, Empty>(
            value =>
            {
                okHandler(value);
                return default;
            },
            error =>
            {
                errorHandler(error);
                return default;
            }
        );
    }

    /// <summary>
    ///     Performs a match on the <see cref="ResultType{T}" />, invoking the
    ///     <see cref="okHandler" /> for an <see cref="OkResultType{T}" />
    ///     and the <see cref="errorHandler" /> for an
    ///     <see cref="ErrorResultType{T}" />
    /// </summary>
    /// <param name="result">The <see cref="ResultType{T}" /></param>
    /// <param name="okHandler">
    ///     The operation to perform for an
    ///     <see cref="OkResultType{T}" />
    /// </param>
    /// <param name="errorHandler">
    ///     The operation to perform for an
    ///     <see cref="ErrorResultType{T}" />
    /// </param>
    /// <exception cref="UnsupportedResultTypeException{TIn}">
    ///     The
    ///     <see cref="ResultType{T}" /> is not one of <see cref="OkResultType{T}" />
    ///     or <see cref="ErrorResultType{T}" />
    /// </exception>
    public static void Match(
        this ResultType<Empty> result,
        Action okHandler,
        Action<Error> errorHandler
    ) => result.Match(_ => okHandler(), errorHandler);
}
