using UnityEngine.UIElements;

public class UI_Menu : UI_Element<UI_Menu>
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Menu");

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
