namespace Draekien.FluentUtils.ValueObject.Samples;

public class Street : ValueObject
{
    public string? Unit { get; set; }
    public string? Number { get; set; }
    public string? Name { get; set;  }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Unit;
        yield return Number;
        yield return Name;
    }
}
