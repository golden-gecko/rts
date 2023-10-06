using UnityEngine;
using UnityEngine.UIElements;

public class GameMenu : MonoBehaviour
{
    public static GameMenu Instance { get; private set; }

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
    }

    void OnEnable()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement rootVisualElement = uiDocument.rootVisualElement;

        panel_Menu = rootVisualElement.Q<VisualElement>("Panel_Menu");

        menu = panel_Menu.Q<Button>("Menu");
        menu.RegisterCallback<ClickEvent>(ev => OnMenu());
    }

    private void OnMenu()
    {
        MainMenu.Instance.gameObject.SetActive(true);
    }

    private VisualElement panel_Menu;

    private Button menu;
}
