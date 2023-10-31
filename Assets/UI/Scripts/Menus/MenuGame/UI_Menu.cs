using UnityEngine.UIElements;

public class UI_Menu : UI_Element<UI_Menu>
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Menu");

        buttonDiplomacy = panel.Q<Button>("Diplomacy");
        buttonDiplomacy.RegisterCallback<ClickEvent>(ev => OnDiplomacy());

        buttonEditor = panel.Q<Button>("Editor");
        buttonEditor.RegisterCallback<ClickEvent>(ev => OnEditor());

        buttonMenu = panel.Q<Button>("Menu");
        buttonMenu.RegisterCallback<ClickEvent>(ev => OnMenu());
    }

    public void OnDiplomacy()
    {
        if (MenuDiplomacy.Instance.Visible)
        {
            MenuGame.Instance.Show(true);
            MenuDiplomacy.Instance.Show(false);
        }
        else
        {
            MenuGame.Instance.Show(false);
            MenuDiplomacy.Instance.Show(true);
        }
    }

    public void OnEditor()
    {
        if (MenuEditor.Instance.Visible)
        {
            MenuGame.Instance.Show(true);
            MenuEditor.Instance.Show(false);
        }
        else
        {
            MenuGame.Instance.Show(false);
            MenuEditor.Instance.Show(true);
        }
    }

    public void OnMenu()
    {
        if (MenuMain.Instance.Visible)
        {
            MenuGame.Instance.Show(true);
            MenuMain.Instance.Show(false);
        }
        else
        {
            MenuGame.Instance.Show(false);
            MenuMain.Instance.Show(true);
        }
    }

    private VisualElement panel;

    private Button buttonDiplomacy;
    private Button buttonEditor;
    private Button buttonMenu;
}
