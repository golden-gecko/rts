using UnityEngine;
using UnityEngine.UIElements;

public class DiplomacyMenu : MonoBehaviour
{
    public static DiplomacyMenu Instance { get; private set; }

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
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement rootVisualElement = uiDocument.rootVisualElement;
    }

    public void Show(bool value)
    {
        GetComponent<UIDocument>().gameObject.SetActive(value);
    }
}
