namespace Draekien.FluentUtils.ValueObject.Samples;

public class Address : ValueObject
{
    public Street Street { get; }
    public string City { get; }
    public string State { get; }
    public string PostCode { get; }

    public Address(
        string? unit,
        string streetNumber,
        string streetName,
        string city,
        string state,
        string postCode)
    {
        Street = new Street(unit, streetNumber, streetName);
        City = city;
        State = state;
        PostCode = postCode;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return PostCode;
    }
}
