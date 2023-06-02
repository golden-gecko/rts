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
        var unload = rootVisualElement.Q<Button>("Unload");

        load.RegisterCallback<ClickEvent>(ev => OnLoad());
        move.RegisterCallback<ClickEvent>(ev => OnMove());
        patrol.RegisterCallback<ClickEvent>(ev => OnPatrol());
        produce.RegisterCallback<ClickEvent>(ev => OnProduce());
        stop.RegisterCallback<ClickEvent>(ev => OnStop());
        unload.RegisterCallback<ClickEvent>(ev => OnUnload());
    }

    void OnLoad()
    {
        hud.Order = OrderType.Load;
    }

    void OnMove()
    {
        hud.Order = OrderType.Move;
    }

    void OnPatrol()
    {
        hud.Order = OrderType.Patrol;
    }

    void OnProduce()
    {
        hud.Order = OrderType.Produce;
    }

    void OnStop()
    {
        hud.Stop();
    }

    void OnUnload()
    {
        hud.Order = OrderType.Unload;
    }

    HUD hud;
}
