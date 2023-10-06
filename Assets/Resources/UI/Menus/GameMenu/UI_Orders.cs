using UnityEngine;
using UnityEngine.UIElements;

public class UI_Orders : MonoBehaviour
{
    void Awake()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement rootVisualElement = uiDocument.rootVisualElement;

        panel = rootVisualElement.Q<VisualElement>("Panel_Orders");
        value = panel.Q<Label>("Value");
    }

    void Update()
    {
        Player activePlayer = HUD.Instance.ActivePlayer;
        MyGameObject hovered = HUD.Instance.Hovered;

        if (hovered != null && hovered.Orders.Count > 0)
        {
            bool ally = hovered.Is(activePlayer, DiplomacyState.Ally);

            panel.style.display = DisplayStyle.Flex;
            value.text = hovered.GetInfoOrders(ally);
        }
        else if (activePlayer.Selection.Count > 0 && activePlayer.Selection.First() != null && activePlayer.Selection.First().Orders.Count > 0)
        {
            panel.style.display = DisplayStyle.Flex;
            value.text = activePlayer.Selection.First().GetInfoOrders(true);
        }
        else
        {
            panel.style.display = DisplayStyle.None;
        }
    }

    private VisualElement panel;
    private Label value;
}
