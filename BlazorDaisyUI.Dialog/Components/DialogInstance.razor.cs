using Microsoft.AspNetCore.Components;

namespace BlazorDaisyUI.Dialog;
public partial class DialogInstance : ComponentBase, IDisposable
{
    private DialogOptions _options = new();
    private readonly string _elementId = "dialog_" + Guid.NewGuid().ToString()[..8];

    [CascadingParameter] private DialogProvider? Parent { get; set; }
    [CascadingParameter] private DialogOptions GlobalDialogOptions { get; set; } = new DialogOptions();

    /// <summary>
    /// User class names, separated by space.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// User styles, applied on top of the component's own classes and styles.
    /// </summary>
    [Parameter]
    public string? Style { get; set; }

    /// <summary>
    /// Use Tag to attach any user data object to the component for your convenience.
    /// </summary>
    [Parameter]
    public object? Tag { get; set; }

    /// <summary>
    /// UserAttributes carries all attributes you add to the component that don't match any of its parameters.
    /// They will be splatted onto the underlying HTML tag.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UserAttributes { get; set; } = new Dictionary<string, object>();

    [Parameter]
    public DialogOptions Options
    {
        get
        {
            if (_options == null)
                _options = new DialogOptions();
            return _options;
        }
        set => _options = value;
    }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public RenderFragment? TitleContent { get; set; }

    [Parameter]
    public RenderFragment? Content { get; set; }

    [Parameter]
    public Guid Id { get; set; }

    /// <summary>
    /// Custom close icon.
    /// </summary>
    [Parameter]
    public string CloseIcon { get; set; } = "✕";

    private string? Position { get; set; }
    private string? DialogMaxWidth { get; set; }
    private string? HeaderBackgroundColor { get; set; }
    private string? HeaderColorContent { get; set; }
    private bool NoHeader { get; set; }
    private bool CloseButton { get; set; }
    private bool FullScreen { get; set; }
    private bool FullWidth { get; set; }


    protected override void OnInitialized()
    {
        ConfigureInstance();
    }

    public void SetOptions(DialogOptions options)
    {
        Options = options;
        ConfigureInstance();
        StateHasChanged();
    }

    public void SetTitle(string title)
    {
        Title = title;
        StateHasChanged();
    }

    /// <summary>
    /// Close and return null. 
    /// 
    /// This is a shorthand of Close(DialogResult.Ok((object)null));
    /// </summary>
    public void Close()
    {
        Close(DialogResult.Ok<object?>(null));
    }

    /// <summary>
    /// Close with dialog result.
    /// 
    /// Usage: Close(DialogResult.Ok(returnValue))
    /// </summary>
    public void Close(DialogResult dialogResult)
    {
        Parent?.DismissInstance(Id, dialogResult);
    }

    /// <summary>
    /// Close and directly pass a return value. 
    /// 
    /// This is a shorthand for Close(DialogResult.Ok(returnValue))
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="returnValue"></param>
    public void Close<T>(T returnValue)
    {
        var dialogResult = DialogResult.Ok<T>(returnValue);
        Parent?.DismissInstance(Id, dialogResult);
    }

    /// <summary>
    /// Cancel the dialog. DialogResult.Canceled will be set to true
    /// </summary>
    public void Cancel()
    {
        Close(DialogResult.Cancel());
    }

    private void ConfigureInstance()
    {
        Position = SetPosition();
        DialogMaxWidth = SetMaxWidth();
        HeaderBackgroundColor = SetHeaderColor();
        HeaderColorContent = SetHeaderColorContent();
        NoHeader = SetHideHeader();
        CloseButton = SetCloseButton();
        FullWidth = SetFullWidth();
        FullScreen = SetFulScreen();
        Class = Classname;
    }

    private string SetPosition()
    {
        DialogPosition position;

        if (Options.Position.HasValue)
        {
            position = Options.Position.Value;
        }
        else if (GlobalDialogOptions.Position.HasValue)
        {
            position = GlobalDialogOptions.Position.Value;
        }
        else
        {
            position = DialogPosition.Center;
        }
        return $"{position.ToDescriptionString()}";
    }

    private string SetMaxWidth()
    {
        MaxWidth maxWidth;

        if (Options.MaxWidth.HasValue)
        {
            maxWidth = Options.MaxWidth.Value;
        }
        else if (GlobalDialogOptions.MaxWidth.HasValue)
        {
            maxWidth = GlobalDialogOptions.MaxWidth.Value;
        }
        else
        {
            maxWidth = MaxWidth.Small;
        }
        return $"{maxWidth.ToDescriptionString()}";
    }

    private string SetHeaderColor()
    {
        DialogHeaderColor headerColor;

        if (Options.HeaderColor.HasValue)
        {
            headerColor = Options.HeaderColor.Value;
        }
        else if (GlobalDialogOptions.HeaderColor.HasValue)
        {
            headerColor = GlobalDialogOptions.HeaderColor.Value;
        }
        else
        {
            headerColor = DialogHeaderColor.Primary;
        }

        return headerColor switch
        {
            DialogHeaderColor.Primary => "bg-primary",
            DialogHeaderColor.Secondary => "bg-secondary",
            DialogHeaderColor.Accent => "bg-accent",
            DialogHeaderColor.Base100 => "bg-base-100",
            DialogHeaderColor.Base200 => "bg-base-200",
            DialogHeaderColor.Base300 => "bg-base-300",
            DialogHeaderColor.Info => "bg-info",
            DialogHeaderColor.Success => "bg-success",
            DialogHeaderColor.Warning => "bg-warning",
            DialogHeaderColor.Error => "bg-error",
            _ => ""
        };
    }

    private string SetHeaderColorContent()
    {
        DialogHeaderColor headerColor;

        if (Options.HeaderColor.HasValue)
        {
            headerColor = Options.HeaderColor.Value;
        }
        else if (GlobalDialogOptions.HeaderColor.HasValue)
        {
            headerColor = GlobalDialogOptions.HeaderColor.Value;
        }
        else
        {
            headerColor = DialogHeaderColor.Primary;
        }

        return headerColor switch
        {
            DialogHeaderColor.Primary => "text-primary-content",
            DialogHeaderColor.Secondary => "text-secondary-content",
            DialogHeaderColor.Accent => "text-accent-content",
            DialogHeaderColor.Base100 => "text-base-content",
            DialogHeaderColor.Base200 => "text-base-content",
            DialogHeaderColor.Base300 => "text-base-content",
            DialogHeaderColor.Info => "text-info-content",
            DialogHeaderColor.Success => "text-success-content",
            DialogHeaderColor.Warning => "text-warning-content",
            DialogHeaderColor.Error => "text-error-content",
            _ => ""
        };
    }

    private bool SetFullWidth()
    {
        if (Options.FullWidth.HasValue)
            return Options.FullWidth.Value;

        if (GlobalDialogOptions.FullWidth.HasValue)
            return GlobalDialogOptions.FullWidth.Value;

        return false;
    }

    private bool SetFulScreen()
    {
        if (Options.FullScreen.HasValue)
            return Options.FullScreen.Value;

        if (GlobalDialogOptions.FullScreen.HasValue)
            return GlobalDialogOptions.FullScreen.Value;

        return false;
    }

    protected string Classname =>
        new CssBuilder("modal-box p-0 overflow-hidden")
            .AddClass(DialogMaxWidth, !FullScreen)
            .AddClass("w-10/12", FullWidth && !FullScreen)
            .AddClass("max-w-full w-screen h-screen max-h-screen rounded-none", FullScreen)
            .AddClass(_dialog?.Class)
            .Build();

    private bool SetHideHeader()
    {
        if (Options.NoHeader.HasValue)
            return Options.NoHeader.Value;

        if (GlobalDialogOptions.NoHeader.HasValue)
            return GlobalDialogOptions.NoHeader.Value;

        return false;
    }

    private bool SetCloseButton()
    {
        if (Options.CloseButton.HasValue)
            return Options.CloseButton.Value;

        if (GlobalDialogOptions.CloseButton.HasValue)
            return GlobalDialogOptions.CloseButton.Value;

        return false;
    }

    private Dialog? _dialog;

    public void Register(Dialog dialog)
    {
        if (dialog == null)
            return;
        _dialog = dialog;
        Class = dialog.Class;
        Style = dialog.Style;
        TitleContent = dialog.TitleContent;
        StateHasChanged();
    }

    public void ForceRender()
    {
        StateHasChanged();
    }

    /// <summary>
    /// Cancels all dialogs in dialog provider collection.
    /// </summary>
    public void CancelAll()
    {
        Parent?.DismissAll();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
