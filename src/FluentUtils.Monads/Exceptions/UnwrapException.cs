namespace FluentUtils.Monads.Exceptions
{
    using System;

    public sealed class UnwrapException : SystemException
    {
        public UnwrapException(string message) : base(message)
        { }

        public UnwrapException(string message, Exception innerException) :
            base(message, innerException)
        { }

        internal static UnwrapException For<T>(None<T> none) =>
            new UnwrapException("Unwrap called on a 'None' option.");
    }
}
