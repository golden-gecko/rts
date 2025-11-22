using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneMenu : UI_Element<SceneMenu>
{
    protected override void Awake()
    {
        base.Awake();

        menu = Root.Q<VisualElement>("Menu");

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

    private VisualElement menu;
}
