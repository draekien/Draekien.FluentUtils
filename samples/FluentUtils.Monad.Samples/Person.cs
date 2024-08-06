namespace FluentUtils.Monad.Samples;

internal record Person(string Name)
{
    public static readonly Person Empty = new(string.Empty);
}
