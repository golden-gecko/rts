using UnityEngine;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    public void Show(bool value)
    {
        GetComponent<UIDocument>().rootVisualElement.visible = value;
    }

    public bool Visible { get => GetComponent<UIDocument>().rootVisualElement.visible; }
}
