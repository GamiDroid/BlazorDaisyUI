using System.ComponentModel;

namespace BlazorDaisyUI.Dialog;
public class DialogOptions
{
    public DialogPosition? Position { get; set; }

    public MaxWidth? MaxWidth { get; set; }

    public DialogHeaderColor? HeaderColor { get; set; }

    public bool? NoHeader { get; set; }
    public bool? CloseButton { get; set; }
    public bool? FullScreen { get; set; }
    public bool? FullWidth { get; set; }
}

public enum MaxWidth
{
    [Description("")]
    None,
    [Description("max-w-xl")]
    Small,
    [Description("max-w-5xl")]
    Medium,
    [Description("max-w-6xl")]
    Large,
}

public enum DialogPosition
{
    [Description("justify-center content-center")]
    Center,
    [Description("justify-start content-center")]
    CenterLeft,
    [Description("justify-end content-center")]
    CenterRight,
    [Description("justify-center content-start")]
    TopCenter,
    [Description("justify-start content-start")]
    TopLeft,
    [Description("justify-end content-start")]
    TopRight,
    [Description("justify-center content-end")]
    BottomCenter,
    [Description("justify-start content-end")]
    BottomLeft,
    [Description("justify-end content-end")]
    BottomRight,
}

public enum DialogHeaderColor
{
    Primary,
    Secondary,
    Accent,
    Neutral,
    Base100,
    Base200,
    Base300,
    Info,
    Success,
    Warning,
    Error
}