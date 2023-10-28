using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : UI_Element<MainMenu>
{
    protected override void Awake()
    {
        base.Awake();

        buttonContinue = GetButton("Continue");
        buttonContinue.RegisterCallback<ClickEvent>(ev => OnButtonContinue());

        buttonNew = GetButton("New");
        buttonNew.RegisterCallback<ClickEvent>(ev => OnButtonNew());

        buttonQuit = GetButton("Quit");
        buttonQuit.RegisterCallback<ClickEvent>(ev => OnButtonQuit());
    }

    private Button GetButton(string name)
    {
        return Root.Q<Button>(name);
    }

    private void OnButtonContinue()
    {
        UI_Menu.Instance.OnMenu();
    }

    private void OnButtonNew()
    {
        Show(false);
        SceneMenu.Instance.Show(true);
    }

    private void OnButtonQuit()
    {
        Application.Quit();
    }

    private Button buttonContinue;
    private Button buttonNew;
    private Button buttonQuit;
}
