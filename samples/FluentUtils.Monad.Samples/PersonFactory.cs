namespace FluentUtils.Monad.Samples;

using Models;

internal static class PersonFactory
{
    public static ResultType<Person> Create(string name)
    {
        if (string.IsNullOrEmpty(name)) return PersonErrors.EmptyName;
        if (name.Length > 255)
        {
            // dynamically generated error code
            return Result.Error<Person>(
                "A name cannot be more than 255 characters long"
            );
        }

        return new Person(name);
    }
}
