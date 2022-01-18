using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FluentUtils.EnumExtensions.UnitTests;

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
