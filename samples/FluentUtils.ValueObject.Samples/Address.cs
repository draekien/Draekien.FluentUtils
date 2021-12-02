namespace FluentUtils.ValueObject.Samples;

public class Address : ValueObject
{
    public Street? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostCode { get; set; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return PostCode;
    }
}
