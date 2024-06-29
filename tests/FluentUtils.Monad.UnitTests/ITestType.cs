namespace FluentUtils.Monad.UnitTests;

internal interface ITestType;

internal sealed record CustomResultType<T> : ResultType<T> where T : notnull;
