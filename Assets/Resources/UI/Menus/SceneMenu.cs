using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneMenu : MonoBehaviour
{
    public static SceneMenu Instance { get; private set; }

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
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement rootVisualElement = uiDocument.rootVisualElement;
        VisualElement menu = rootVisualElement.Q<VisualElement>("Menu");

        string[] scenes = new string[]
        {
            "Level_001",
            "Test_Attack",
            "Test_Construction_Structure",
            "Test_Construction_Unit",
            "Test_Transport",
        };

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
