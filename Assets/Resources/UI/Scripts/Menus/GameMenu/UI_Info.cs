using UnityEngine.UIElements;

public class UI_Info : UI_Element<UI_Info>
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Info");
        value = panel.Q<Label>("Value");
    }

    private void Update()
    {
        Player activePlayer = HUD.Instance.ActivePlayer;
        MyGameObject hovered = HUD.Instance.Hovered;

        if (hovered != null)
        {
            bool ally = hovered.Is(activePlayer, DiplomacyState.Ally);

            panel.style.display = DisplayStyle.Flex;
            value.text = hovered.GetInfo(ally);
        }
        else if (activePlayer.Selection.Count > 0 && activePlayer.Selection.First() != null)
        {
            panel.style.display = DisplayStyle.Flex;
            value.text = activePlayer.Selection.First().GetInfo(true);
        }
        else
        {
            panel.style.display = DisplayStyle.None;
        }
    }

    private VisualElement panel;
    private Label value;
}
