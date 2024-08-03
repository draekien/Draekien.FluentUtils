namespace FluentUtils.Monad.Samples;

using Extensions;
using Models;

internal static class PersonGenerator
{
    public static IEnumerable<Person> Generate(int number)
    {
        for (var i = 0; i < number; i++)
        {
            ResultType<Person> createPersonResult =
                PersonFactory.Create($"Person {i}");

            yield return createPersonResult.Match(
                person =>
                {
                    Console.WriteLine("Person created");
                    return person;
                },
                e =>
                {
                    Console.WriteLine((string?)e.ToString());
                    return Person.Empty;
                }
            );
        }
    }
}
