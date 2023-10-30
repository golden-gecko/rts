using UnityEngine;
using UnityEngine.UIElements;

public class MenuEditor : UI_Element<MenuScene>
{
    protected override void Awake()
    {
        base.Awake();

        Blueprints = Root.Q<VisualElement>("Blueprints");
        Parts = Root.Q<VisualElement>("Parts");
        Preview = Root.Q<VisualElement>("Preview");

        ButtonOK = Root.Q<Button>("OK");
        ButtonOK.RegisterCallback<ClickEvent>(ev => OnButtonOK());

        ButtonCancel = Root.Q<Button>("Cancel");
        ButtonCancel.RegisterCallback<ClickEvent>(ev => OnButtonCancel());

        /*
        visualElement = Root.Q<VisualElement>("VisualElement");

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

            visualElement.Add(buttonContainer);
        }
        */
    }

    private void OnButtonOK()
    {
    }

    private void OnButtonCancel()
    {
    }

    [SerializeField]
    private VisualTreeAsset TemplatePart;

    private VisualElement Blueprints;
    private VisualElement Parts;
    private VisualElement Preview;

    private Button ButtonOK;
    private Button ButtonCancel;
}
