using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Collections.ObjectModel;

namespace BlazorDaisyUI.Dialog;
public partial class DialogProvider : ComponentBase, IDisposable
{
    [Inject] private IDialogService? DialogService { get; set; }
    [Inject] private NavigationManager? NavigationManager { get; set; }

    [Parameter] public bool? NoHeader { get; set; }
    [Parameter] public bool? CloseButton { get; set; }
    [Parameter] public bool? DisableBackdropClick { get; set; }
    [Parameter] public bool? CloseOnEscapeKey { get; set; }
    [Parameter] public bool? FullWidth { get; set; }
    [Parameter] public DialogPosition? Position { get; set; }
    [Parameter] public MaxWidth? MaxWidth { get; set; }

    private readonly Collection<IDialogReference> _dialogs = new();
    private readonly DialogOptions _globalDialogOptions = new();

    protected override void OnInitialized()
    {
        if (DialogService is not null)
        {
            DialogService.OnDialogInstanceAdded += AddInstance;
            DialogService.OnDialogCloseRequested += DismissInstance;
        }

        if (NavigationManager is not null)
            NavigationManager.LocationChanged += LocationChanged;

        _globalDialogOptions.CloseButton = CloseButton;
        _globalDialogOptions.NoHeader = NoHeader;
        _globalDialogOptions.Position = Position;
        _globalDialogOptions.FullWidth = FullWidth;
        _globalDialogOptions.MaxWidth = MaxWidth;
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            foreach (var dialogReference in _dialogs.Where(x => !x.Result.IsCompleted))
            {
                dialogReference.RenderCompleteTaskCompletionSource.TrySetResult(true);
            }
        }

        return base.OnAfterRenderAsync(firstRender);
    }

    internal void DismissInstance(Guid id, DialogResult result)
    {
        var reference = GetDialogReference(id);
        if (reference != null)
            DismissInstance(reference, result);
    }

    private void AddInstance(IDialogReference dialog)
    {
        _dialogs.Add(dialog);
        StateHasChanged();
    }

    public void DismissAll()
    {
        _dialogs.ToList().ForEach(r => DismissInstance(r, DialogResult.Cancel()));
        StateHasChanged();
    }

    private void DismissInstance(IDialogReference dialog, DialogResult result)
    {
        if (!dialog.Dismiss(result)) return;

        _dialogs.Remove(dialog);
        StateHasChanged();
    }

    private IDialogReference? GetDialogReference(Guid id)
    {
        return _dialogs.SingleOrDefault(x => x.Id == id);
    }

    private void LocationChanged(object? sender, LocationChangedEventArgs args)
    {
        DismissAll();
    }

    public void Dispose()
    {
        if (NavigationManager != null)
            NavigationManager.LocationChanged -= LocationChanged;

        if (DialogService != null)
        {
            DialogService.OnDialogInstanceAdded -= AddInstance;
            DialogService.OnDialogCloseRequested -= DismissInstance;
        }

        GC.SuppressFinalize(this);
    }
}