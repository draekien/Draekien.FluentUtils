namespace FluentUtils.Monads
{
    using System;
    using JetBrains.Annotations;

    /// <summary>Some value of type <typeparamref name="T" /></summary>
    /// <typeparam name="T">
    /// The type belonging to the value inside the
    /// <see cref="Some{T}" />
    /// </typeparam>
    public sealed class Some<T> : IOption<T>
    {
        internal Some(T value)
        {
            if (Equals(value, default(T)))
            {
                throw new InvalidOperationException(
                    $"The value of a `Some` option cannot be it's default value `{default(T)}`");
            }

            Value = value;
        }

        [NotNull]
        private T Value { get; }

        /// <inheritdoc />
        public bool IsSome => true;

        /// <inheritdoc />
        public bool IsNone => false;

        /// <inheritdoc />
        public bool IsSomeAnd(Predicate<T> predicate) =>
            predicate(Value);

        /// <inheritdoc />
        public bool IsNoneOr(Predicate<T> predicate) =>
            predicate(Value);

        /// <inheritdoc />
        public TOut Match<TOut>(
            Func<T, TOut> onSome,
            Func<TOut> onNone) => onSome(Value);

        /// <inheritdoc />
        public void Match(Action<T> onSome, Action onNone)
        {
            onSome(Value);
        }

        /// <inheritdoc />
        public T Expect(string message) =>
            Value;

        /// <inheritdoc />
        public T Unwrap() => Value;

        /// <inheritdoc />
        public T UnwrapOr(T value) =>
            Value;

        /// <inheritdoc />
        public T UnwrapOrDefault() =>
            Value;

        /// <inheritdoc />
        public T UnwrapOrElse(Func<T> @else) =>
            Value;

        /// <inheritdoc />
        public IOption<T2> Map<T2>(Func<T, T2> map) =>
            Option.Some(map(Value));

        /// <inheritdoc />
        public T2 MapOr<T2>(T2 @default, Func<T, T2> map) =>
            Match(map, () => @default);

        /// <inheritdoc />
        public T2 MapOrElse<T2>(
            Func<T2> createDefault,
            Func<T, T2> map) => Match(map, createDefault);

        /// <inheritdoc />
        public IOption<T> Inspect(Action<T> action)
        {
            action(Value);
            return this;
        }

        /// <inheritdoc />
        public IOption<T> Filter(Predicate<T> predicate) =>
            predicate(Value) ? this : Option.None<T>();

        /// <inheritdoc />
        public IOption<T> Or(IOption<T> other) =>
            this;

        /// <inheritdoc />
        public IOption<T> OrElse(Func<IOption<T>> createElse) =>
            this;

        /// <inheritdoc />
        public IOption<T> Xor(IOption<T> other) =>
            other.IsSome ? Option.None<T>() : this;

        /// <inheritdoc />
        public IOption<(T, T2)> Zip<T2>(IOption<T2> other)
        {
            if (other is Some<T2> otherSome)
            {
                return Option.Some((Value, otherSome.Value));
            }

            return Option.None<(T, T2)>();
        }
    }
}
