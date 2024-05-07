﻿namespace FluentUtils.Monad.Samples;

public record Person(string Name)
{
    public static readonly Person Empty = new(string.Empty);
}

public static class PersonErrors
{
    public static readonly Error EmptyName = new("P01", "A person must have a name.");
}

public static class PersonFactory
{
    public static ResultType<Person> Create(string name)
    {
        if (string.IsNullOrEmpty(name)) return PersonErrors.EmptyName;

        return new Person(name);
    }
}

public class PersonGenerator
{
    public IEnumerable<Person> Generate(int number)
    {
        for (var i = 0; i < number; i++)
        {
            var createPersonResult = PersonFactory.Create($"Person {i}");

            yield return createPersonResult.Match(
                person =>
                {
                    Console.WriteLine("Person created");
                    return person;
                },
                e =>
                {
                    Console.WriteLine(e.ToString());
                    return Person.Empty;
                });
        }
    }
}