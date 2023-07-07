using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    private void Awake()
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

    private void Start()
    {
        GetButton("New").RegisterCallback<ClickEvent>(ev => OnButtonNew());
        GetButton("Quit").RegisterCallback<ClickEvent>(ev => OnButtonQuit());
    }

    private void OnButtonScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    private void OnButtonNew()
    {
        gameObject.SetActive(false);
        SceneMenu.Instance.gameObject.SetActive(true);
    }

    private void OnButtonQuit()
    {
        Application.Quit();
    }

    private Button GetButton(string name)
    {
        return GetComponent<UIDocument>().rootVisualElement.Q<Button>(name);
    }
}
