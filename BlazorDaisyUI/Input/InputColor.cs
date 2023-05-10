using System.ComponentModel;

namespace BlazorDaisyUI;
public enum InputColor
{
    None,
    [Description("input-primary")] Primary,
    [Description("input-secondary")] Secondary,
    [Description("input-accent")] Accent,
    [Description("input-info")] Info,
    [Description("input-success")] Success,
    [Description("input-warning")] Warning,
    [Description("input-error")] Error,
}