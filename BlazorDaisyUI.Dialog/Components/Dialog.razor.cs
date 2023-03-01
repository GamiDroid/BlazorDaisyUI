using Microsoft.AspNetCore.Components;

namespace BlazorDaisyUI.Dialog;
public partial class Dialog : ComponentBase
{
    protected string ContentClass => new CssBuilder()
          .AddClass(ClassContent)
        .Build();

    protected string ActionClass => new CssBuilder("flex justify-end gap-1 p-2")
      .AddClass(ClassActions)
    .Build();

    [CascadingParameter] private DialogInstance? DialogInstance { get; set; }

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

    [Inject] public IDialogService? DialogService { get; set; }

    /// <summary>
    /// Define the dialog title as a renderfragment (overrides Title)
    /// </summary>
    [Parameter]
    public RenderFragment? TitleContent { get; set; }

    /// <summary>
    /// Define the dialog body here
    /// </summary>
    [Parameter]
    public RenderFragment? DialogContent { get; set; }

    /// <summary>
    /// Define the action buttons here
    /// </summary>
    [Parameter]
    public RenderFragment? DialogActions { get; set; }

    /// <summary>
    /// Default options to pass to Show(), if none are explicitly provided.
    /// Typically useful on inline dialogs.
    /// </summary>
    [Parameter]
    public DialogOptions? Options { get; set; }

    /// <summary>
    /// No padding at the sides
    /// </summary>
    [Parameter]
    public bool DisableSidePadding { get; set; }

    /// <summary>
    /// CSS class that will be applied to the dialog content
    /// </summary>
    [Parameter]
    public string? ClassContent { get; set; }

    /// <summary>
    /// CSS class that will be applied to the action buttons container
    /// </summary>
    [Parameter]
    public string? ClassActions { get; set; }

    /// <summary>
    /// CSS styles to be applied to the dialog content
    /// </summary>
    [Parameter]
    public string? ContentStyle { get; set; }

    /// <summary>
    /// Bind this two-way to show and close an inlined dialog. Has no effect on opened dialogs
    /// </summary>
    [Parameter]
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible == value)
                return;
            _isVisible = value;
            IsVisibleChanged.InvokeAsync(value);
        }
    }
    private bool _isVisible;

    /// <summary>
    /// Raised when the inline dialog's display status changes.
    /// </summary>
    [Parameter] public EventCallback<bool> IsVisibleChanged { get; set; }

    private bool IsInline => DialogInstance == null;

    private IDialogReference? _reference;

    /// <summary>
    /// Show this inlined dialog
    /// </summary>
    /// <param name="title"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public IDialogReference Show(string? title = null, DialogOptions? options = null)
    {
        if (!IsInline)
            throw new InvalidOperationException("You can only show an inlined dialog.");
        if (DialogService is null)
            throw new InvalidOperationException("Instance of DialogService cannot be found.");
        if (_reference != null)
            Close();
        var parameters = new DialogParameters()
        {
            [nameof(Class)] = Class,
            [nameof(Style)] = Style,
            [nameof(Tag)] = Tag,
            [nameof(TitleContent)] = TitleContent,
            [nameof(DialogContent)] = DialogContent,
            [nameof(DialogActions)] = DialogActions,
            [nameof(DisableSidePadding)] = DisableSidePadding,
            [nameof(ClassContent)] = ClassContent,
            [nameof(ClassActions)] = ClassActions,
            [nameof(ContentStyle)] = ContentStyle,
        };

        _reference = DialogService.Show<Dialog>(title, parameters, options ??= Options = new());
        _reference.Result.ContinueWith(t =>
        {
            _isVisible = false;
            InvokeAsync(() => IsVisibleChanged.InvokeAsync(false));
        });
        return _reference;
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (IsInline)
        {
            if (_isVisible && _reference == null)
            {
                Show(); // if isVisible and we don't have any reference we need to call Show
            }
            else if (_reference != null)
            {
                if (IsVisible)
                    (_reference.Dialog as Dialog)?.ForceUpdate(); // forward render update to instance
                else
                    Close(); // if we still have reference but it's not visible call Close
            }
        }
        base.OnAfterRender(firstRender);
    }

    /// <summary>
    /// Used for forwarding state changes from inlined dialog to its instance
    /// </summary>
    internal void ForceUpdate()
    {
        StateHasChanged();
    }

    /// <summary>
    /// Close the currently open inlined dialog
    /// </summary>
    /// <param name="result"></param>
    public void Close(DialogResult? result = null)
    {
        if (!IsInline || _reference == null)
            return;
        _reference.Close(result);
        _reference = null;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        DialogInstance?.Register(this);
    }
}
