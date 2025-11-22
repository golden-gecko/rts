using UnityEngine;
using UnityEngine.UIElements;

public class UI_Menu : MonoBehaviour
{
    public static UI_Menu Instance { get; private set; } // TODO: Remove singleton from this class.

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement rootVisualElement = uiDocument.rootVisualElement;

        panel = rootVisualElement.Q<VisualElement>("Panel_Menu");

        buttonMenu = panel.Q<Button>("Menu");
        buttonMenu.RegisterCallback<ClickEvent>(ev => OnMenu());
    }

    public void OnMenu() // TODO: Make private.
    {
        if (MainMenu.Instance.Visible)
        {
            GameMenu.Instance.Show(true);
            MainMenu.Instance.Show(false);
        }
        else
        {
            GameMenu.Instance.Show(false);
            MainMenu.Instance.Show(true);
        }
    }

    private VisualElement panel;

    private Button buttonMenu;
}
