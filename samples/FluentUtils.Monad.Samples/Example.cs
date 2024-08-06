namespace FluentUtils.Monad.Samples;

using Extensions;

internal class Example
{
    public void Execute()
    {
        ResultType<Person> createPersonResult =
            Result.Bind(() => new Person("Bob Smith"));

        ResultType<string> getPersonNameResult =
            createPersonResult.Pipe(person => person.Name);

        ResultType<string> ensureNameIsNotWhitespaceResult =
            getPersonNameResult.Ensure(
                x => !string.IsNullOrWhiteSpace(x));

        string output = ensureNameIsNotWhitespaceResult.Match(
            x => $"Hello {x}",
            error =>
            {
                Console.WriteLine(error.Message);
                return "Who are you?";
            });

        Console.WriteLine(output);
    }
}
