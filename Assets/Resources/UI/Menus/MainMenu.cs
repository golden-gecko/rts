using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
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
    }

    void OnEnable()
    {
        GetButton("Continue").RegisterCallback<ClickEvent>(ev => OnButtonContinue());
        GetButton("New").RegisterCallback<ClickEvent>(ev => OnButtonNew());
        GetButton("Quit").RegisterCallback<ClickEvent>(ev => OnButtonQuit());
    }

    public void Show(bool value)
    {
        GetComponent<UIDocument>().gameObject.SetActive(value);
    }

    private Button GetButton(string name)
    {
        return GetComponent<UIDocument>().rootVisualElement.Q<Button>(name);
    }

    private void OnButtonContinue()
    {
        Show(false);
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
}
