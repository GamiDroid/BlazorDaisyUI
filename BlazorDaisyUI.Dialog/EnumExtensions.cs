using System.ComponentModel;

namespace BlazorDaisyUI.Dialog;
internal static class EnumExtensions
{
    public static string ToDescriptionString(this Enum val)
    {
        var field = val.GetType().GetField(val.ToString());
        if (field is null)
            throw new ArgumentException($"Unable to get field named '{val.ToString()}' from Enum '{val.GetType()}'.");

        var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length > 0
            ? attributes[0].Description
            : val.ToString().ToLower();
    }
}
