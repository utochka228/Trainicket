using UnityEngine;

public abstract class Menu<T> : Menu where T : Menu<T>
{
    public static T i { get; private set; }

    protected virtual void Awake() {
        i = (T)this;
    }

    protected virtual void OnDestroy() {
        i = null;
    }

    protected static void Open() {
        if (i == null)
            UIMenuManager.Instance.CreateInstance<T>();
        else
            i.gameObject.SetActive(true);

        UIMenuManager.Instance.OpenMenu(i);
    }

    protected static void Close() {
        if (i == null) {
            Debug.LogErrorFormat("Trying to close menu {0} but Instance is null", typeof(T));
            return;
        }

        UIMenuManager.Instance.CloseMenu(i);
    }

    public override void OnBackPressed() {
        Close();
    }
}

public abstract class Menu : MonoBehaviour
{
    [Tooltip("Destroy the Game Object when menu is closed (reduces memory usage)")]
    public bool DestroyWhenClosed = true;

    [Tooltip("Disable menus that are under this one in the stack")]
    public bool DisableMenusUnderneath = true;

    [Tooltip("Mark this like true when you wanna do it independent and not so important")]
    public bool VisualHelper = false;
    public abstract void OnBackPressed();
}
