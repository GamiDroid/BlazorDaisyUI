namespace BlazorDaisyUI.Dialog;
public class CssBuilder
{
    public CssBuilder()
    {
    }

    public CssBuilder(string? cssClass)
    {
        _ = AddClass(cssClass);
    }

    public CssBuilder AddClass(string? cssClass)
    {
        if (!string.IsNullOrWhiteSpace(cssClass))
            _cssClasses.Add(cssClass.Trim());
        return this;
    }

    public CssBuilder AddClass(string? cssClass, bool addWhen)
    {
        if (addWhen)
            return AddClass(cssClass);
        return this;
    }

    public string Build() => string.Join(" ", _cssClasses);

    private readonly HashSet<string> _cssClasses = new();
}
