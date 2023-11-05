using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuScene : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        /*
        Menu = Root.Q<VisualElement>("Menu");

        int sceneCount = SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }

        foreach (string scene in scenes)
        {
            TemplateContainer container = templateButton.Instantiate();
            Button button = container.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnButtonScene(scene));
            button.text = scene;
            button.userData = scene;

            Menu.Add(container);
        }
        */
    }

    private void OnButtonScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    [SerializeField]
    private VisualTreeAsset templateButton;

    private VisualElement Menu;
}
