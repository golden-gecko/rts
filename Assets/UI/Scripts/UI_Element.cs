using UnityEngine;
using UnityEngine.UIElements;

public class UI_Element<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();

        Document = GetComponent<UIDocument>();
        Root = Document.rootVisualElement;
    }

    public void Show(bool value)
    {
        Root.visible = value;
    }

    public bool Visible { get => Root.visible; }

    protected UIDocument Document { get; private set; }
    protected VisualElement Root { get; private set; }
}
