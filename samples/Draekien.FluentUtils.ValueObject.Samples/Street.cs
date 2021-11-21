namespace Draekien.FluentUtils.ValueObject.Samples;

public class Street : ValueObject
{
    public string? Unit { get; }
    public string Number { get; }
    public string Name { get; }

    public Street(
        string? unit,
        string number,
        string name)
    {
        Unit = unit;
        Number = number;
        Name = name;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Unit;
        yield return Number;
        yield return Name;
    }
}
