namespace FluentUtils.Monad;

public static partial class Results
{
    public static ResultType<T> Error<T>(Error error) =>
        new ErrorResultType<T>(error);

    public static ResultType<Empty> Error(Error error) => Error<Empty>(error);
}
