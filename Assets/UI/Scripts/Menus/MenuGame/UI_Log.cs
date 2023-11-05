using UnityEngine.UIElements;

public class UI_Log : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Log");
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
