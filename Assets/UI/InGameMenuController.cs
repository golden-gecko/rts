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

        var load = rootVisualElement.Q<Button>("Load");
        var move = rootVisualElement.Q<Button>("Move");
        var patrol = rootVisualElement.Q<Button>("Patrol");
        var produce = rootVisualElement.Q<Button>("Produce");
        var stop = rootVisualElement.Q<Button>("Stop");
        var transport = rootVisualElement.Q<Button>("Transport");
        var unload = rootVisualElement.Q<Button>("Unload");

        var heavyFactory = rootVisualElement.Q<Button>("HeavyFactory");
        var lightFactory = rootVisualElement.Q<Button>("LightFactory");
        var radar = rootVisualElement.Q<Button>("Radar");
        var refinery = rootVisualElement.Q<Button>("Refinery");
        var researchLab = rootVisualElement.Q<Button>("ResearchLab");
        var storage = rootVisualElement.Q<Button>("Storage");

        load.RegisterCallback<ClickEvent>(ev => OnOrder(OrderType.Load));
        move.RegisterCallback<ClickEvent>(ev => OnOrder(OrderType.Move));
        patrol.RegisterCallback<ClickEvent>(ev => OnOrder(OrderType.Patrol));
        produce.RegisterCallback<ClickEvent>(ev => OnOrder(OrderType.Produce));
        stop.RegisterCallback<ClickEvent>(ev => OnOrder(OrderType.Stop));
        transport.RegisterCallback<ClickEvent>(ev => OnOrder(OrderType.Transport));
        unload.RegisterCallback<ClickEvent>(ev => OnOrder(OrderType.Unload));

        heavyFactory.RegisterCallback<ClickEvent>(ev => OnConstruct("HeavyFactory"));
        lightFactory.RegisterCallback<ClickEvent>(ev => OnConstruct("LightFactory"));
        radar.RegisterCallback<ClickEvent>(ev => OnConstruct("Radar"));
        refinery.RegisterCallback<ClickEvent>(ev => OnConstruct("Refinery"));
        researchLab.RegisterCallback<ClickEvent>(ev => OnConstruct("ResearchLab"));
        storage.RegisterCallback<ClickEvent>(ev => OnConstruct("Storage"));
    }

    void OnConstruct(string blueprint)
    {
        hud.Order = OrderType.Construct;
        hud.Blueprint = blueprint;
    }

    void OnOrder(OrderType orderType)
    {
        if (orderType == OrderType.Stop)
        {
            hud.Stop();
        }
        else
        {
            hud.Order = OrderType.Load;
        }
    }

    HUD hud;
}
