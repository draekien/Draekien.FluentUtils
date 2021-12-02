# FluentUtils.ValueObject

## Example Usage

Inherit `ValueObject` and implement the `GetEqualityComponents` method.

```csharp
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
```
