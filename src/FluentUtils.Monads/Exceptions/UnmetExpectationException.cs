namespace FluentUtils.Monads.Exceptions
{
    using System;

    public sealed class UnmetExpectationException : SystemException
    {
        public UnmetExpectationException(string message) : base(message)
        { }

        public UnmetExpectationException(
            string message,
            Exception innerException) : base(message, innerException)
        { }

        internal static UnmetExpectationException For<TValue>(
            string message,
            TValue value) =>
            new UnmetExpectationException($"{message}: {value}");
    }
}
