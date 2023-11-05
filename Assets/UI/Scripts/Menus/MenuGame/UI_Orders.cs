using UnityEngine.UIElements;

public class UI_Orders : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Orders");
        value = panel.Q<Label>("Value");
    }

    private void Update()
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
