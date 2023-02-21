using System.ComponentModel;

namespace BlazorDaisyUI.Dialog;
public class DialogOptions
{
    public DialogPosition? Position { get; set; }

    public MaxWidth? MaxWidth { get; set; }

    public bool? NoHeader { get; set; }
    public bool? CloseButton { get; set; }
    public bool? FullScreen { get; set; }
    public bool? FullWidth { get; set; }
}

public enum MaxWidth
{
    [Description("small")]
    Small,
}

public enum DialogPosition
{
    [Description("center")]
    Center,
    [Description("centerleft")]
    CenterLeft,
    [Description("centerright")]
    CenterRight,
    [Description("topcenter")]
    TopCenter,
    [Description("topleft")]
    TopLeft,
    [Description("topright")]
    TopRight,
    [Description("bottomcenter")]
    BottomCenter,
    [Description("bottomleft")]
    BottomLeft,
    [Description("bottomright")]
    BottomRight,
    [Description("custom")]
    Custom
}
