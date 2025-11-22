using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneMenu : MonoBehaviour
{
    public static SceneMenu Instance { get; private set; }

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

    protected void OnEnable()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement rootVisualElement = uiDocument.rootVisualElement;
        VisualElement menu = rootVisualElement.Q<VisualElement>("Menu");

        int sceneCount = SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }

        foreach (string scene in scenes)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnButtonScene(scene));
            button.text = scene;
            button.userData = scene;

            menu.Add(buttonContainer);
        }
    }

    private void OnButtonScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    [SerializeField]
    private VisualTreeAsset templateButton;
}
