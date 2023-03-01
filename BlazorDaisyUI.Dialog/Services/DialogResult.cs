namespace BlazorDaisyUI.Dialog;
public class DialogResult
{
    public object Data { get; }
    public Type DataType { get; }
    public bool Canceled { get; }

    protected internal DialogResult(object data, Type resultType, bool canceled)
    {
        Data = data;
        DataType = resultType;
        Canceled = canceled;
    }

    public static DialogResult Ok<T>(T result) => Ok(result, default);

    public static DialogResult Ok<T>(T result, Type dialogType) => new(result, dialogType, false);

    public static DialogResult Cancel() => new(default, typeof(object), true);
}