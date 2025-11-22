using UnityEngine;
using UnityEngine.UIElements;

public class InGameMenuController : MonoBehaviour
{
    void Start()
    {
        var gameObject = GameObject.FindGameObjectWithTag("HUD");

        if (gameObject)
        {
            hud = gameObject.GetComponent<HUD>();
        }

        var uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;

        var extract = rootVisualElement.Q<Button>("Extract");
        var move = rootVisualElement.Q<Button>("Move");
        var stop = rootVisualElement.Q<Button>("Stop");

        extract.RegisterCallback<ClickEvent>(ev => OnExtract());
        move.RegisterCallback<ClickEvent>(ev => OnMove());
        stop.RegisterCallback<ClickEvent>(ev => OnStop());
    }

    void Update()
    {
    }

    void OnExtract()
    {
    }

    void OnMove()
    {
    }

    void OnStop()
    {
        foreach (var item in hud.Selected)
        {
            item.Stop();
        }
    }

    HUD hud;
}
