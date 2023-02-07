namespace FluentUtils.DomainDrivenEnums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

/// <summary>
/// Represents an enumeration member with domain specific properties
/// </summary>
[PublicAPI]
public abstract class DomainDrivenEnum : IComparable<DomainDrivenEnum>, IEquatable<DomainDrivenEnum>
{
    /// <summary>
    /// A record which represents an enumeration member with domain specific properties
    /// </summary>
    /// <param name="value">The index value of the enumeration instance</param>
    /// <param name="displayName">The display name of the enumeration instance</param>
    protected DomainDrivenEnum(int value, string displayName)
    {
        Value = value;
        DisplayName = displayName;
    }

    /// <summary>
    /// The index value of the enumeration instance
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// The display name of the enumeration instance
    /// </summary>
    public string DisplayName { get; }

    /// <inheritdoc />
    public int CompareTo(DomainDrivenEnum? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;

        return Value.CompareTo(other.Value);
    }

    /// <inheritdoc />
    public bool Equals(DomainDrivenEnum? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return Value.Equals(other.Value);
    }

    /// <summary>
    /// Gets all declared instances of domain driven enums for a given TEnum type
    /// </summary>
    /// <typeparam name="TEnum">The domain driven enum type</typeparam>
    /// <returns>All declared instances of the domain driven enum type</returns>
    public static IEnumerable<TEnum> GetAll<TEnum>() where TEnum : DomainDrivenEnum
    {
        Type type = typeof(TEnum);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        IEnumerable<object> values = fields.Select(field => field.GetValue(null));

        return values.Cast<TEnum>();
    }

    /// <summary>
    /// Parses the provided value to provide the matching instance of TEnum
    /// </summary>
    /// <param name="value">The index value to parse</param>
    /// <typeparam name="TEnum">The domain driven enum type</typeparam>
    /// <returns>The parsed instance of TEnum</returns>
    public static TEnum FromValue<TEnum>(int value) where TEnum : DomainDrivenEnum
    {
        TEnum matching = Parse<TEnum, int>(value, "value", item => item.Value == value);

        return matching;
    }

    /// <summary>
    /// Parses the provided display name to provide the matching instance of TEnum
    /// </summary>
    /// <param name="value">The display name to parse</param>
    /// <typeparam name="TEnum">The domain driven enum type</typeparam>
    /// <returns>THe parsed instance of TEnum</returns>
    public static TEnum FromDisplayName<TEnum>(string value) where TEnum : DomainDrivenEnum
    {
        TEnum matching = Parse<TEnum, string>(
            value,
            "display name",
            item => item.DisplayName.Equals(value, StringComparison.OrdinalIgnoreCase));

        return matching;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return DisplayName;
    }

    private static TDomainDrivenEnum Parse<TDomainDrivenEnum, TValue>(
        TValue value,
        string description,
        Func<TDomainDrivenEnum, bool> predicate)
        where TDomainDrivenEnum : DomainDrivenEnum
    {
        TDomainDrivenEnum? matching = GetAll<TDomainDrivenEnum>().FirstOrDefault(predicate);

        if (matching is null)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                $"'{value}' is not a valid {description} in {typeof(TDomainDrivenEnum)}.");
        }

        return matching;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not DomainDrivenEnum other) return false;
        if (GetType() != obj.GetType()) return false;

        return Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value ^ 41;
    }

    /// <summary>
    /// Compares two instances of <see cref="DomainDrivenEnum" /> to see if they are equal
    /// </summary>
    /// <param name="left">The value on the left side of the equality operator</param>
    /// <param name="right">The value on the right side of the equality operator</param>
    /// <returns>true if they are equal</returns>
    public static bool operator ==(DomainDrivenEnum? left, DomainDrivenEnum? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Compares two instances of <see cref="DomainDrivenEnum" /> to see if they are not equal
    /// </summary>
    /// <param name="left">The value on the left side of the inequality operator</param>
    /// <param name="right">The value on the right side of the inequality operator</param>
    /// <returns>true if they are not equal</returns>
    public static bool operator !=(DomainDrivenEnum? left, DomainDrivenEnum? right)
    {
        return !Equals(left, right);
    }
}
