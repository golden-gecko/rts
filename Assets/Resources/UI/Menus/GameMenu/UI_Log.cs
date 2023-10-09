using UnityEngine;
using UnityEngine.UIElements;

public class UI_Log : MonoBehaviour
{
    public static UI_Log Instance { get; private set; } // TODO: Remove singleton from this class.

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

        panel = rootVisualElement.Q<VisualElement>("Panel_Log");
        value = panel.Q<Label>("Value");

        Log("");
    }

    public void Log(string message)
    {
        value.text = message;
    }

    private VisualElement panel;
    private Label value;
}
