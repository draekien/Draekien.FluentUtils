using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentUtils.ValueObject.Samples;
using FluentAssertions;
using Xunit;

namespace FluentUtils.ValueObject.UnitTests;

public class ValueObjectFacts
{
    private readonly IFixture _fixture;

    public ValueObjectFacts()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void GivenTwoIdenticalValueObjects_WhenInvokingEquals_ThenReturnTrue()
    {
        // ARRANGE
        var leftStreet = _fixture.Create<Street>();
        var rightStreet = _fixture.Build<Street>()
                                  .With(x => x.Name, leftStreet.Name)
                                  .With(x => x.Unit, leftStreet.Unit)
                                  .With(x => x.Number, leftStreet.Number)
                                  .Create();

        // ACT
        bool result = leftStreet.Equals(rightStreet);

        // ASSERT
        result.Should()
              .BeTrue();
    }

    [Fact]
    public void GivenNullOtherObject_WhenInvokingEquals_ThenReturnFalse()
    {
        // ARRANGE
        var leftAddress = _fixture.Create<Address>();

        // ACT
        bool result = leftAddress.Equals(null);

        // ASSERT
        result.Should()
              .BeFalse();
    }

    [Fact]
    public void GivenOtherObjectWithDifferentType_WhenInvokingEquals_ThenReturnFalse()
    {
        // ARRANGE
        var leftAddress = _fixture.Create<Address>();
        var right = new
        {
            Id = 0
        };

        // ACT
        bool result = leftAddress.Equals(right);

        // ASSERT
        result.Should()
              .BeFalse();
    }

    [Fact]
    public void GivenDifferentValueObject_WhenInvokingEquals_ThenReturnFalse()
    {
        // ARRANGE
        var leftAddress = _fixture.Create<Address>();
        var rightStreet = _fixture.Create<Street>();

        // ACT
        // ReSharper disable once SuspiciousTypeConversion.Global
        bool result = leftAddress.Equals(rightStreet);

        // ASSERT
        result.Should()
              .BeFalse();
    }

    [Fact]
    public void GivenTwoValueObjectsWithDifferentValues_WhenInvokingEquals_ThenReturnFalse()
    {
        List<Street> streets = _fixture.CreateMany<Street>(2).ToList();

        // ACT
        bool result = streets.First().Equals(streets.Last());

        // ASSERT
        result.Should()
              .BeFalse();
    }

    [Fact]
    public void GivenTwoIdenticalValueObjects_WhenInvokingOperatorEquals_ThenReturnTrue()
    {
        // ARRANGE
        var leftStreet = _fixture.Create<Street>();
        var rightStreet = _fixture.Build<Street>()
                                  .With(x => x.Name, leftStreet.Name)
                                  .With(x => x.Unit, leftStreet.Unit)
                                  .With(x => x.Number, leftStreet.Number)
                                  .Create();

        // ACT
        bool result = leftStreet == rightStreet;

        // ASSERT
        result.Should()
              .BeTrue();
    }

    [Fact]
    public void GivenTwoDifferentValueObjects_WhenInvokingOperatorEquals_ThenReturnFalse()
    {
        // ARRANGE
        List<Street> streets = _fixture.CreateMany<Street>(2).ToList();

        // ACT
        bool result = streets.First() == streets.Last();

        // ASSERT
        result.Should()
              .BeFalse();
    }

    [Fact]
    public void GivenOneNullValue_WhenInvokingOperatorEquals_ThenReturnFalse()
    {
        // ARRANGE
        var leftStreet = _fixture.Create<Street>();

        // ACT
        bool result = leftStreet == null;

        // ASSERT
        result.Should()
              .BeFalse();
    }

    [Fact]
    public void GivenOneNullValue_WhenInvokingOperatorNotEquals_ThenReturnTrue()
    {
        // ARRANGE
        var leftStreet = _fixture.Create<Street>();

        // ACT
        bool result = leftStreet != null;

        // ASSERT
        result.Should()
              .BeTrue();
    }

    [Fact]
    public void WhenGettingHashcode_ThenReturnValue()
    {
        // ARRANGE
        var leftStreet = _fixture.Create<Street>();

        // ACT
        int? result = leftStreet.GetHashCode();

        // ASSERT
        result.Should().NotBeNull();
    }
}
