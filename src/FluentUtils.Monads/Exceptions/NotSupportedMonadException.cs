namespace FluentUtils.Monads.Exceptions
{
    using System;

    public sealed class NotSupportedMonadException : NotSupportedException
    {
        public NotSupportedMonadException(string message) : base(message)
        { }

        public NotSupportedMonadException(
            string message,
            Exception innerException) : base(message, innerException)
        { }
    }
}
