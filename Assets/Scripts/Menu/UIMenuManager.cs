using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    public delegate void OnProduct();
    public static event OnProduct onProductAdded;

    public Stack<Menu> menuStack = new Stack<Menu>();
    public static UIMenuManager Instance { get; set; }
    public AuthMenu authMenu;
    public SearchMenu searchMenu;
    public LoadingMenu loadingMenu;
    public RegisterMenu registerMenu;
    public SideMenu sideMenu;
    public string accesstoken;

    List<Menu> visualHelpers = new List<Menu>();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
    }
    private void Start() {
        Application.targetFrameRate = 60;

        AuthMenu.Show();
    }

    public void CreateInstance<T>() where T : Menu
    {
        var prefab = GetPrefab<T>();

        Instantiate(prefab, transform);
    }

    public void RemoveAllVisualHelpers() {
        for (int i = 0; i < visualHelpers.Count; i++) {
            Destroy(visualHelpers[i].gameObject);
        }
        visualHelpers.Clear();
    }

    public void RemoveVisualHelper(Menu helper) {
        visualHelpers.Remove(helper);
        Destroy(helper.gameObject);
    }
    public void RemoveVisualHelper(Menu[] helpers) {
        for (int i = 0; i < helpers.Length; i++) {
            visualHelpers.Remove(helpers[i]);
            Destroy(helpers[i].gameObject);
        }
    }
    public void OpenMenu(Menu instance)
    {
        if (instance.VisualHelper) {
            visualHelpers.Add(instance);
            return;
        }
        // De-activate top menu
        if (menuStack.Count > 0)
        {
            if (instance.DisableMenusUnderneath)
            {
                foreach (var menu in menuStack)
                {
                    menu.gameObject.SetActive(false);

                    if (menu.DisableMenusUnderneath)
                        break;
                }
            }

            var topCanvas = instance.GetComponent<Canvas>();
            var previousCanvas = menuStack.Peek().GetComponent<Canvas>();
            if (topCanvas.sortingOrder <= previousCanvas.sortingOrder + 1)
            {
                topCanvas.sortingOrder = previousCanvas.sortingOrder + 1;
            }
        }

        menuStack.Push(instance);
    }

    private T GetPrefab<T>() where T : Menu
    {
        // Get prefab dynamically, based on public fields set from Unity
        // You can use private fields with SerializeField attribute too
        var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var field in fields)
        {
            var prefab = field.GetValue(this) as T;
            if (prefab != null)
            {
                return prefab;
            }
        }

        throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    }

    public void CloseMenu(Menu menu)
    {
        if (menu.VisualHelper) {
            RemoveVisualHelper(menu);
            return;
        }
        if (menuStack.Count == 0)
        {
            Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", menu.GetType());
            return;
        }

        if (menuStack.Peek() != menu)
        {
            Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", menu.GetType());
            return;
        }

        CloseTopMenu();
    }

    public void CloseTopMenu()
    {
        var instance = menuStack.Pop();

        if (instance.DestroyWhenClosed)
            Destroy(instance.gameObject);
        else
            instance.gameObject.SetActive(false);

        // Re-activate top menu
        // If a re-activated menu is an overlay we need to activate the menu under it
        foreach (var menu in menuStack)
        {
            menu.gameObject.SetActive(true);

            if (menu.DisableMenusUnderneath)
                break;
        }
    }
    public void CloseTopMenus(int i)
    {
        for (int j = 0; j < i; j++)
        {
            CloseTopMenu();
        }
    }
    public void ChangeLocalization(string localization) {
        LocalizationManager.instance.setLocalization(localization);
    }
    private void Update()
    {
        // On Android the back button is sent as Esc
        if (Input.GetKeyDown(KeyCode.Escape) && menuStack.Count > 0)
        {
            menuStack.Peek().OnBackPressed();
        }
    }
}
