namespace FluentUtils.Monad;

/// <summary>
///     Extensions for <see cref="ErrorResultType{T}" />
/// </summary>
public static class ErrorResultTypeExtensions
{
    /// <summary>
    ///     Converts a <see cref="ErrorResultType{T}" /> of type <see cref="TIn" /> to
    ///     a type of <see cref="TOut" />, preserving the error inside the result
    /// </summary>
    /// <param name="result">The <see cref="ErrorResultType{T}" /> to be converted</param>
    /// <typeparam name="TIn">The input type</typeparam>
    /// <typeparam name="TOut">The output type</typeparam>
    /// <returns>A <see cref="ErrorResultType{T}" /> where T is the output type</returns>
    public static ErrorResultType<TOut> To<TIn, TOut>(
        this ErrorResultType<TIn> result
    ) where TIn : notnull where TOut : notnull =>
        new(result.Error);
}
