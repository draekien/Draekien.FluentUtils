namespace FluentUtils.Monads
{
    /// <summary>Static methods for <see cref="IResult{TOk,TErr}" /></summary>
    public static class Result<TOk, TErr>
    {
        public static IResult<TOk, TErr> Ok(TOk value) =>
            new Ok<TOk, TErr>(value);

        public static IResult<TOk, TErr> Err(TErr value) =>
            new Err<TOk, TErr>(value);
    }

    /// <summary>Static methods for <see cref="IResult{TOk,TErr}" /></summary>
    public static class Result
    {
        /// <summary>
        /// Converts from <c>IResult&lt;IResult&lt;TOk, TErr&gt;, TErr&gt;</c> to
        /// <c>IResult&lt;TOk, TErr&gt;</c>
        /// </summary>
        /// <remarks>Flattening only removes one level of nesting at a time.</remarks>
        /// <param name="result">The result to flatten.</param>
        /// <typeparam name="TOk">The <see cref="Ok{TOk,TErr}" /> value type</typeparam>
        /// <typeparam name="TErr">The <see cref="Err{TOk,TErr}" /> value type</typeparam>
        public static IResult<TOk, TErr> Flatten<TOk, TErr>(
            this IResult<IResult<TOk, TErr>, TErr> result) =>
            result.Match(
                inner => inner,
                Result<TOk, TErr>.Err);


        /// <summary>
        /// Transposes a <c>result</c> of an <c>option</c> into an <c>option</c>
        /// of a <c>result</c>
        /// </summary>
        /// <list type="bullet">
        /// <item>
        /// <see cref="Ok{TOk,TErr}" /> of <see cref="None{T}" /> will be mapped to
        /// <see cref="None{T}" />.
        /// </item>
        /// <item>
        /// <see cref="Ok{TOk,TErr}" /> of <see cref="Some{T}" /> and
        /// <see cref="Err{TOk,TErr}" /> will be mapped to <see cref="Some{T}" /> of
        /// <see cref="Ok{TOk,TErr}" /> and <see cref="Some{T}" /> of
        /// <see cref="Err{TOk,TErr}" />
        /// </item>
        /// </list>
        public static IOption<IResult<TOk, TErr>> Transpose<TOk, TErr>(
            this IResult<IOption<TOk>, TErr> result) =>
            result.Match(
                option =>
                {
                    return option.Match(
                        value => Option.Some(Result<TOk, TErr>.Ok(value)),
                        Option.None<IResult<TOk, TErr>>);
                },
                err => Option.Some(Result<TOk, TErr>.Err(err)));
    }
}
