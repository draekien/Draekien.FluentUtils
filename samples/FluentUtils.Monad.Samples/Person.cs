namespace FluentUtils.Monad.Samples.Models;

internal record Person(string Name)
{
    public static readonly Person Empty = new(string.Empty);
}
