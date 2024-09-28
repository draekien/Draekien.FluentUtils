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
    public static Error FailedPredicate(
        string predicate,
        Exception? exception = null) => new(
        "FUME_01",
        $"Result value does not match predicate '{predicate}'.",
        exception);

    /// <summary>
    ///     Failed to bind the result of a factory method to a result
    /// </summary>
    public static Error FailedToBindFactory(
        Exception exception,
        string factoryExpression) =>
        new(
            "FUME_02",
            $"Error when binding result for factory '{factoryExpression}'.",
            exception);

    /// <summary>
    ///     Failed to pipe the value of a result via a pipe expression
    /// </summary>
    /// <param name="exception">The <see cref="Exception" /></param>
    /// <param name="pipeExpression">The pipe expression</param>
    /// <returns>The <see cref="Error" /></returns>
    public static Error FailedToPipeValue(
        Exception exception,
        string pipeExpression) => new(
        "FUME_03",
        $"Error piping value via expression '{pipeExpression}'.",
        exception);

    /// <summary>
    ///     Failed to tap the value of a result
    /// </summary>
    /// <param name="exception">The <see cref="Exception" /></param>
    /// <param name="tapExpression">The tap expression</param>
    /// <returns>The <see cref="Error" /></returns>
    public static Error FailedToTapValue(
        Exception exception,
        string tapExpression) => new(
        "FUME_04",
        $"Error taping value via expression '{tapExpression}'.",
        exception);
}
