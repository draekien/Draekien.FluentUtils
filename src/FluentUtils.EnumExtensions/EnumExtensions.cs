using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FluentUtils.EnumExtensions;

/// <summary>
/// A set of useful enum extensions
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the name value of the display attribute on an enum value.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <returns>The display name if it exists, or the enum value as a string.</returns>
    public static string GetDisplayName<TEnum>(this TEnum enumValue)
        where TEnum : Enum
    {
        DisplayAttribute? displayAttribute = GetAttribute<DisplayAttribute, TEnum>(enumValue);

        return displayAttribute?.GetName() ?? enumValue.ToString();
    }

    /// <summary>
    /// Gets the description from a description attribute on an enum value.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <returns>The description if it exists, or the enum value as a string.</returns>
    public static string GetDescription<TEnum>(this TEnum enumValue)
        where TEnum : Enum
    {
        DescriptionAttribute? descriptionAttribute = GetAttribute<DescriptionAttribute, TEnum>(enumValue);

        return descriptionAttribute?.Description ?? enumValue.ToString();
    }

    /// <summary>
    /// Gets the specified attribute from an enum value.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <typeparam name="TAttribute">The attribute type.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <returns>The attribute.</returns>
    private static TAttribute? GetAttribute<TAttribute, TEnum>(TEnum enumValue)
        where TAttribute : Attribute
        where TEnum : Enum
    {
        Type type = enumValue.GetType();
        MemberInfo[] members = type.GetMember(enumValue.ToString());
        MemberInfo? member = members.FirstOrDefault();

        var attribute = member?.GetCustomAttribute<TAttribute>();

        return attribute;
    }
}
