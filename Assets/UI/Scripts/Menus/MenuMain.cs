using UnityEngine;
using UnityEngine.UIElements;

public class MenuMain : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        buttonContinue = GetButton("Continue");
        buttonContinue.RegisterCallback<ClickEvent>(x => OnButtonContinue());

        buttonNew = GetButton("New");
        buttonNew.RegisterCallback<ClickEvent>(x => OnButtonNew());

        buttonQuit = GetButton("Quit");
        buttonQuit.RegisterCallback<ClickEvent>(x => OnButtonQuit());
    }

    private Button GetButton(string name)
    {
        return Root.Q<Button>(name);
    }

    private void OnButtonContinue()
    {
        UI.Instance.GoToMenu(MenuType.Game);
    }

    private void OnButtonNew()
    {
        UI.Instance.GoToMenu(MenuType.Scene);
    }

    private void OnButtonQuit()
    {
        Application.Quit();
    }

    private Button buttonContinue;
    private Button buttonNew;
    private Button buttonQuit;
}
