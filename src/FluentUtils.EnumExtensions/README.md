# FluentUtils.EnumExtensions

## Example Usage

Get the display name or description from an enum.

```csharp
public enum ExampleEnum
{
    ValueOnly,
    [Display(Name = "Named Enum")]
    WithName,
    [Description("This enum value has a description")]
    WithDescription,
    [Display(Name = "Descriptive Enum")]
    [Description("Lorem Ipsum")]
    WithNameAndDescription
}

// returns "ValueOnly" as if you called "ToString()"
string valueOnly = ExampleEnum.ValueOnly.GetDisplayName();

// returns "Named Enum"
string displayName = ExampleEnum.WithName.GetDisplayName();

// returns "Descriptive Enum"
string description = ExampleEnum.WithDescription.GetDescription();
```
