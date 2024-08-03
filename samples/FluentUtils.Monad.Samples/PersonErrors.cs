namespace FluentUtils.Monad.Samples;

internal static class PersonErrors
{
    public static readonly Error EmptyName = new(
        "P01",
        "A person must have a name."
    );
}
