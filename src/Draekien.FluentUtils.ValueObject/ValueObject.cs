namespace Draekien.FluentUtils.ValueObject;

/// <summary>
///     An object that is immutable and has no identity. See this article for a more in depth explanation:
///     https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    ///     Checks to see if the provided object is equal to the current instance of value object.
    /// </summary>
    /// <param name="other">The object to compare with the current value object.</param>
    /// <returns>True if the to be compared object equals the current value object.</returns>
    public override bool Equals(object? other)
    {
        if (other is null || other.GetType() != GetType())
            return false;

        return other is ValueObject valueObject && GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    /// <summary>
    ///     Checks to see if the provided value object is equal to the current instance of value object.
    /// </summary>
    /// <param name="other">The value object to compare with the current value object.</param>
    /// <returns>True if the to be compared value object equals the current value object.</returns>
    public bool Equals(ValueObject? other) =>
        other is not null && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    /// <summary>
    ///     Gets the hashcode of the current value object.
    /// </summary>
    /// <returns>The hashcode.</returns>
    public override int GetHashCode() =>
        GetEqualityComponents()
            .Select(x => x is not null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);

    /// <summary>
    ///     Checks to see if the left value object is equal to the right value object.
    /// </summary>
    /// <param name="left">The object on the left of the equals operator.</param>
    /// <param name="right">The object on the right of the equals operator.</param>
    /// <returns>True when the left object equals the right object.</returns>
    public static bool operator ==(ValueObject? left, ValueObject? right) => EqualOperator(left, right);

    /// <summary>
    ///     Checks to see if the left value object does not equal to the right value object.
    /// </summary>
    /// <param name="left">The object on the left of the equals operator.</param>
    /// <param name="right">The object on the right of the equals operator.</param>
    /// <returns>True when the left object does not equal to the right object.</returns>
    public static bool operator !=(ValueObject? left, ValueObject? right) => NotEqualOperator(left, right);

    /// <summary>
    ///     Gets the object properties to be used in the equality operations.
    /// </summary>
    /// <remarks>
    ///     Use `yield return` to return each component of the value object.
    /// </remarks>
    /// <returns>An enumerable of object components.</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    ///     Checks to see if one value object is equal to another value object.
    /// </summary>
    /// <param name="left">The object on the left of the equals operator.</param>
    /// <param name="right">The object on the right of the equals operator.</param>
    /// <returns>True when the left value equals the right value.</returns>
    private static bool EqualOperator(ValueObject? left, ValueObject? right)
    {
        if (left is null ^ right is null) return false;

        return left is null || left.Equals(right);
    }

    /// <summary>
    ///     Checks to see if one value object is not equal to another value object.
    /// </summary>
    /// <param name="left">The object on the left of the not equals operator.</param>
    /// <param name="right">The object on the right of the not equals operator.</param>
    /// <returns>True when the left value does not equal to the right value.</returns>
    private static bool NotEqualOperator(ValueObject? left, ValueObject? right) => !EqualOperator(left, right);
}
