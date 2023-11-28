using UnityEngine.UIElements;

public class UI_Menu : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Menu");

        buttonDiplomacy = panel.Q<Button>("Diplomacy");
        buttonDiplomacy.RegisterCallback<ClickEvent>(x => OnDiplomacy());

        buttonEditor = panel.Q<Button>("Editor");
        buttonEditor.RegisterCallback<ClickEvent>(x => OnEditor());

        buttonMenu = panel.Q<Button>("Menu");
        buttonMenu.RegisterCallback<ClickEvent>(x => OnMenu());
    }

    private void OnDiplomacy()
    {
        UI.Instance.GoToMenu(MenuType.Diplomacy);
    }

    private void OnEditor()
    {
        UI.Instance.GoToMenu(MenuType.Editor);
    }

    private void OnMenu()
    {
        UI.Instance.GoToMenu(MenuType.Main);
    }

    private VisualElement panel;

    private Button buttonDiplomacy;
    private Button buttonEditor;
    private Button buttonMenu;
}
