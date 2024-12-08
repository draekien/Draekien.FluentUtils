namespace FluentUtils.Monads.Exceptions
{
    using System;

    public class UnwrapException : SystemException
    {
        public UnwrapException(string message) : base(message)
        { }

        public UnwrapException(string message, Exception innerException) :
            base(message, innerException)
        { }

        internal static UnwrapException<TErr>
            For<TOk, TErr>(Err<TOk, TErr> err) =>
            new UnwrapException<TErr>(
                "Unwrap called on an `Error` result.",
                err.Value);

        internal static UnwrapException<TOk> For<TOk, TErr>(Ok<TOk, TErr> ok) =>
            new UnwrapException<TOk>(
                "Unwrap called on an `Ok` result.",
                ok.Value);
    }

    public sealed class UnwrapException<T> : UnwrapException
    {
        /// <inheritdoc />
        public UnwrapException(string message, T value) : base(message)
        {
            Value = value;
        }

        /// <inheritdoc />
        public UnwrapException(
            string message,
            T value,
            Exception innerException) : base(message, innerException)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
