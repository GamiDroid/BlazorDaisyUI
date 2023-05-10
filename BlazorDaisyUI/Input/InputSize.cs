using System.ComponentModel;

namespace BlazorDaisyUI;
public enum InputSize
{
    None,
    [Description("input-md")] Medium,
    [Description("input-lg")] Large,
    [Description("input-sm")] Small,
    [Description("input-xs")] ExtraSmall,
}
