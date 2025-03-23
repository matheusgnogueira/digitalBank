using System.ComponentModel;
using System.Reflection;

namespace DigitalBank.Util.Extensions;

public static class EnumExtension
{
    public static string? GetDescription(this Enum value)
    {
        if (value == null)
            return null;

        Type type = value.GetType();
        string? name = Enum.GetName(type, value);

        if (name != null)
        {
            FieldInfo? field = type.GetField(name);

            if (field != null && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                return attr.Description;
        }

        return null;
    }

    public static bool EqualsAny<TEnum>(this TEnum value, params TEnum[] values) where TEnum : Enum
    {
        return values.Contains(value);
    }

    public static bool EqualsAny<TEnum>(this TEnum value, params int[] intValues) where TEnum : Enum
    {
        int enumValue = Convert.ToInt32(value);
        return intValues.Contains(enumValue);
    }

    public static IEnumerable<string> GetDescriptions(this Enum value, bool allowNull = false)
    {
        var enumType = value.GetType();
        var values = Enum.GetValues(enumType).Cast<Enum>();

        foreach (var v in values.Where(value.HasFlag))
        {
            var description = v.GetDescription();

            if (allowNull || !string.IsNullOrEmpty(description))
            {
                yield return description ?? string.Empty;
            }
        }
    }
}
