namespace FluentUtils.Monad;

/// <summary>
///     Built in errors
/// </summary>
[PublicAPI]
public static class MonadErrors
{
    /// <summary>
    ///     The value of a result does not match a predicate
    /// </summary>
    public static readonly Error FailedPredicate = new(
        "FM_01",
        "Result value does not match predicate.");

    /// <summary>
    ///     Failed to bind the result of a factory method to a result
    /// </summary>
    public static Error FailedToBindFactory(Exception exception) =>
        new(
            "FM_02",
            "Error when binding the result of the provided factory method.",
            exception);
}
