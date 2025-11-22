using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private void OnButtonScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    private void OnButtonQuit()
    {
        Application.Quit();
    }

    private void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement rootVisualElement = uiDocument.rootVisualElement;

        menu = rootVisualElement.Q<VisualElement>("MainMenu");

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

            menu.Insert(menu.childCount - 1, buttonContainer); // TODO: Move to level submenu.
        }

        rootVisualElement.Q<Button>("Quit").RegisterCallback<ClickEvent>(ev => OnButtonQuit());
    }

    public VisualTreeAsset templateButton;

    private VisualElement menu;
}
