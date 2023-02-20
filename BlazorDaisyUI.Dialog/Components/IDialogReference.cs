using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BlazorDaisyUI.Dialog;
public interface IDialogReference
{
    Guid Id { get; }

    RenderFragment? RenderFragment { get; set; }

    Task<DialogResult> Result { get; }

    TaskCompletionSource<bool> RenderCompleteTaskCompletionSource { get; }

    void Close();

    void Close(DialogResult result);

    bool Dismiss(DialogResult result);

    object? Dialog { get; }

    void InjectRenderFragment(RenderFragment rf);

    void InjectDialog(object inst);

    Task<T?> GetReturnValueAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>();
}
