namespace FluentUtils.Monads
{
    /// <summary>
    ///     Static functions for <see cref="IOption{T}" />
    /// </summary>
    public static class Option
    {
        /// <summary>
        ///     Creates a <see cref="Some{T}" />
        /// </summary>
        /// <param name="value">The value of the <see cref="Some{T}" /></param>
        /// <typeparam name="T">The option value's type.</typeparam>
        /// <returns>An <see cref="IOption{T}" />.</returns>
        public static IOption<T> Some<T>(T value) => new Some<T>(value);

        /// <summary>
        ///     Creates a <see cref="None{T}" />
        /// </summary>
        /// <typeparam name="T">The option value's type.</typeparam>
        /// <returns>An <see cref="IOption{T}" />.</returns>
        public static IOption<T> None<T>() => new None<T>();

        /// <summary>
        ///     Unzips an option containing a tuple of two options.
        /// </summary>
        /// <param name="option">The option to be unzipped.</param>
        /// <typeparam name="T1">The first option value's type.</typeparam>
        /// <typeparam name="T2">The second option value's type.</typeparam>
        /// <returns>
        ///     If <paramref name="option" /> is <c>Some&lt;(T1, T2)&gt;</c> this method
        ///     returns <c>(Some&lt;T1&gt;, Some&lt;T2&gt;)</c>. Otherwise
        ///     <c>(None&lt;T1&gt;, None&lt;T2&gt;)</c> is returned.
        /// </returns>
        public static (IOption<T1>, IOption<T2>) Unzip<T1, T2>(
            this IOption<(T1, T2)> option) => option.Match(
            tuple => (Some(tuple.Item1), Some(tuple.Item2)),
            () => (None<T1>(), None<T2>()));

        /// <summary>
        ///     Converts from <c>Option&lt;Option&lt;T&gt;&gt;</c> to
        ///     <c>Option&lt;T&gt;</c>.
        /// </summary>
        /// <remarks>Flattening only removes one level of nesting at a time.</remarks>
        /// <param name="option">The option that needs to be flattened.</param>
        /// <typeparam name="T">The option value's type.</typeparam>
        public static IOption<T> Flatten<T>(this IOption<IOption<T>> option) =>
            option.Match(innerOption => innerOption, None<T>);
    }
}
