using UnityEngine;
using UnityEngine.UIElements;

public class UI_Element<T> : Singleton<T> where T : Component
{
    protected override void Awake()
    {
        base.Awake();

        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
    }

    public void Show(bool value)
    {
        root.visible = value;
    }

    public bool Visible { get => root.visible; }

    protected UIDocument document { get; private set; }
    protected VisualElement root { get; private set; }
}
