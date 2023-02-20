using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace BlazorDaisyUI.Dialog;
public class DialogReference : IDialogReference
{
    private readonly TaskCompletionSource<DialogResult> _resultCompletion = new();

    private readonly IDialogService _dialogService;

    public DialogReference(Guid dialogInstanceId, IDialogService dialogService)
    {
        Id = dialogInstanceId;
        _dialogService = dialogService;
    }

    public void Close()
    {
        _dialogService.Close(this);
    }

    public void Close(DialogResult result)
    {
        _dialogService.Close(this, result);
    }

    public virtual bool Dismiss(DialogResult result)
    {
        return _resultCompletion.TrySetResult(result);
    }

    public Guid Id { get; }

    public object? Dialog { get; private set; }
    public RenderFragment? RenderFragment { get; set; }

    public Task<DialogResult> Result => _resultCompletion.Task;

    TaskCompletionSource<bool> IDialogReference.RenderCompleteTaskCompletionSource { get; } = new();

    public void InjectDialog(object inst)
    {
        Dialog = inst;
    }

    public void InjectRenderFragment(RenderFragment rf)
    {
        RenderFragment = rf;
    }

    public async Task<T?> GetReturnValueAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>()
    {
        var result = await Result;
        try
        {
            return (T)result.Data;
        }
        catch (InvalidCastException)
        {
            Debug.WriteLine($"Could not cast return value to {typeof(T)}, returning default.");
            return default;
        }
    }
}
