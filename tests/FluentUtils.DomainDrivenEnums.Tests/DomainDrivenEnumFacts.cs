namespace FluentUtils.DomainDrivenEnums.Tests;

using Samples;

public class DomainDrivenEnumFacts
{
    [Fact]
    public void WhenGettingAllDeclaredInstances_ThenReturnExpectedCollection()
    {
        // Act
        List<CustomerType> members = DomainDrivenEnum.GetAll<CustomerType>().ToList();

        // Assert
        members.Should().ContainEquivalentOf(CustomerType.Premium);
        members.Should().ContainEquivalentOf(CustomerType.Standard);
        members.Should().ContainEquivalentOf(CustomerType.Vip);
    }

    [Fact]
    public void GivenValueIsInEnum_WhenCreatingEnumFromValue_ThenReturnExpectedEnumMember()
    {
        // Arrange
        int value = CustomerType.Standard.Value;

        // Act
        var result = DomainDrivenEnum.FromValue<CustomerType>(value);

        // Assert
        result.Should().BeEquivalentTo(CustomerType.Standard);
    }

    [Fact]
    public void GivenValueIsNotInEnum_WhenCreatingEnumFromValue_ThenThrowArgumentOutOfRangeException()
    {
        // Arrange
        const int value = 999;
        Action action = () => DomainDrivenEnum.FromValue<CustomerType>(value);

        // Act + Assert
        action.Should()
              .Throw<ArgumentOutOfRangeException>()
              .WithParameterName("value")
              .WithMessage($"'{value}' is not a valid value in {typeof(CustomerType)}. (Parameter 'value')");
    }

    [Fact]
    public void GivenDisplayNameIsInEnum_WhenCreatingEnumFromDisplayName_ThenReturnExpectedEnumMember()
    {
        // Arrange
        string displayName = CustomerType.Standard.DisplayName;

        // Act
        var result = DomainDrivenEnum.FromDisplayName<CustomerType>(displayName);

        // Assert
        result.Should().BeEquivalentTo(CustomerType.Standard);
    }

    [Fact]
    public void GivenDisplayNameIsNotInEnum_WhenCreatingEnumFromDisplayName_ThenThrowArgumentOutOfRangeException()
    {
        // Arrange
        const string displayName = "does not exist";
        Action action = () => DomainDrivenEnum.FromDisplayName<CustomerType>(displayName);

        // Act + Assert
        action.Should()
              .Throw<ArgumentOutOfRangeException>()
              .WithParameterName("value")
              .WithMessage(
                   $"'{displayName}' is not a valid display name in {typeof(CustomerType)}. (Parameter 'value')");
    }

    [Fact]
    public void WhenInvokingToString_ThenReturnDisplayName()
    {
        // Arrange
        string expected = CustomerType.Standard.DisplayName;

        // Act
        var result = CustomerType.Standard.ToString();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void GivenTheSameEnumValues_WhenCheckingToSeeIfTheyAreEqual_ThenReturnTrue()
    {
        // Arrange
        CustomerType first = CustomerType.Standard;
        CustomerType second = CustomerType.Standard;

        // Act
        bool result = first.Equals(second);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GivenDifferentEnumValues_WhenCheckingToSeeIfTheyAreEqual_ThenReturnFalse()
    {
        // Arrange
        CustomerType first = CustomerType.Standard;
        CustomerType second = CustomerType.Premium;

        // Act
        bool result = first.Equals(second);

        // Assert
        result.Should().BeFalse();
    }
}
