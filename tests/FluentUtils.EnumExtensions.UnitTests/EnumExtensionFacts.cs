using FluentAssertions;
using Xunit;

namespace FluentUtils.EnumExtensions.UnitTests;

public class EnumExtensionFacts
{
    [Fact]
    public void GivenNoNameAndNoDescription_WhenGettingDisplayName_ThenReturnEnumValueAsString()
    {
        // Arrange
        const ExampleEnum exampleEnum = ExampleEnum.ValueOnly;

        // Act
        string result = exampleEnum.GetDisplayName();

        // Assert
        result.Should().Be(exampleEnum.ToString());
    }

    [Fact]
    public void GivenNoNameAndNoDescription_WhenGettingDescription_ThenReturnEnumValueAsString()
    {
        // Arrange
        const ExampleEnum exampleEnum = ExampleEnum.ValueOnly;

        // Act
        string result = exampleEnum.GetDescription();

        // Assert
        result.Should().Be(exampleEnum.ToString());
    }

    [Fact]
    public void GivenName_WhenGettingDisplayName_ThenReturnName()
    {
        // Arrange
        const ExampleEnum exampleEnum = ExampleEnum.WithName;

        // Act
        string result = exampleEnum.GetDisplayName();

        // Assert
        result.Should().Be("Named Enum");
    }

    [Fact]
    public void GivenDescription_WhenGettingDescription_ThenReturnDescription()
    {
        // Arrange
        const ExampleEnum exampleEnum = ExampleEnum.WithDescription;

        // Act
        string result = exampleEnum.GetDescription();

        // Assert
        result.Should().Be("This enum value has a description");
    }
}
