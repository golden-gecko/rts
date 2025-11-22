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

        var move = rootVisualElement.Q<Button>("Move");
        var patrol = rootVisualElement.Q<Button>("Patrol");
        var produce = rootVisualElement.Q<Button>("Produce");
        var stop = rootVisualElement.Q<Button>("Stop");

        move.RegisterCallback<ClickEvent>(ev => OnMove());
        patrol.RegisterCallback<ClickEvent>(ev => OnPatrol());
        produce.RegisterCallback<ClickEvent>(ev => OnProduce());
        stop.RegisterCallback<ClickEvent>(ev => OnStop());
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

    HUD hud;
}
