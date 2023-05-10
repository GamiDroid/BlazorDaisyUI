using System.ComponentModel;
using System.Reflection;

namespace BlazorDaisyUI;

public static class EnumHelper
{
    public static string? GetDescription(this Enum enumValue)
    {
        var strValue = enumValue.ToString();
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        if (fieldInfo is null)
            return strValue;

        var descriptionAttributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>(inherit: false).ToList();
        if (!descriptionAttributes.Any())
            return strValue;

        return descriptionAttributes[0].Description;
    }
}
