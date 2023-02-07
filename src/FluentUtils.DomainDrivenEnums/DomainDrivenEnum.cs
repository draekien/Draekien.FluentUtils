namespace FluentUtils.DomainDrivenEnums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

/// <summary>
/// A record which represents an enumeration member with domain specific properties
/// </summary>
/// <param name="Value">The index value of the enumeration instance</param>
/// <param name="DisplayName">The display name of the enumeration instance</param>
[PublicAPI]
public abstract record DomainDrivenEnum(int Value, string DisplayName) : IComparable<DomainDrivenEnum>
{
    /// <summary>
    /// The index value of the enumeration instance
    /// </summary>
    public int Value { get; } = Value;

    /// <summary>
    /// The display name of the enumeration instance
    /// </summary>
    public string DisplayName { get; } = DisplayName;

    /// <inheritdoc />
    public int CompareTo(DomainDrivenEnum? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;

        return Value.CompareTo(other.Value);
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
    /// <param name="displayName">The display name to parse</param>
    /// <typeparam name="TEnum">The domain driven enum type</typeparam>
    /// <returns>THe parsed instance of TEnum</returns>
    public static TEnum FromDisplayName<TEnum>(string displayName) where TEnum : DomainDrivenEnum, new()
    {
        TEnum matching = Parse<TEnum, string>(
            displayName,
            "display name",
            item => item.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase));

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

        if (matching == null)
        {
            throw new InvalidOperationException(
                $"'{value}' is not a valid {description} in {typeof(TDomainDrivenEnum)}.");
        }

        return matching;
    }
}
