using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;

        rootVisualElement.Q<Button>("New").RegisterCallback<ClickEvent>(ev => OnButtonNew());
        rootVisualElement.Q<Button>("Quit").RegisterCallback<ClickEvent>(ev => OnButtonQuit());
    }

    void OnButtonNew()
    {
        SceneManager.LoadScene("Level_001", LoadSceneMode.Single);
    }

    void OnButtonQuit()
    {
        Application.Quit();
    }
}
