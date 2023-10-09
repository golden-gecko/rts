using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : Menu
{
    public static MainMenu Instance { get; private set; }

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

        buttonContinue = GetButton("Continue");
        buttonContinue.RegisterCallback<ClickEvent>(ev => OnButtonContinue());

        buttonNew = GetButton("New");
        buttonNew.RegisterCallback<ClickEvent>(ev => OnButtonNew());

        buttonQuit = GetButton("Quit");
        buttonQuit.RegisterCallback<ClickEvent>(ev => OnButtonQuit());
    }

    void Start()
    {
        buttonContinue = GetButton("Continue");
        buttonContinue.RegisterCallback<ClickEvent>(ev => OnButtonContinue());

        buttonNew = GetButton("New");
        buttonNew.RegisterCallback<ClickEvent>(ev => OnButtonNew());

        buttonQuit = GetButton("Quit");
        buttonQuit.RegisterCallback<ClickEvent>(ev => OnButtonQuit());
    }

    void OnEnable()
    {
        buttonContinue = GetButton("Continue");
        buttonContinue.RegisterCallback<ClickEvent>(ev => OnButtonContinue());

        buttonNew = GetButton("New");
        buttonNew.RegisterCallback<ClickEvent>(ev => OnButtonNew());

        buttonQuit = GetButton("Quit");
        buttonQuit.RegisterCallback<ClickEvent>(ev => OnButtonQuit());
    }

    private Button GetButton(string name)
    {
        var i = GetComponent<UIDocument>();
        var j = i.rootVisualElement;

        return j.Q<Button>(name);
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
