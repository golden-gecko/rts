using UnityEngine;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    protected virtual void Awake()
    {
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
