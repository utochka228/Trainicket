/// <summary>
/// A base menu class that implements parameterless Show and Hide methods
/// </summary>
public abstract class MenuItem<T> : Menu<T> where T : MenuItem<T>
{
    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }
}
