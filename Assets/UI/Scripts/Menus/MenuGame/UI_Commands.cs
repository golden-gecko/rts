using UnityEngine;
using UnityEngine.UIElements;

public class UI_Commands : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Commands");
    }

    private void Update()
    {
        Player activePlayer = HUD.Instance.ActivePlayer;
        MyGameObject hovered = HUD.Instance.Hovered;

        if (hovered != null)
        {
            bool ally = hovered.Is(activePlayer, DiplomacyState.Ally);

            if (ally)
            {
                panel.style.display = DisplayStyle.Flex;
            }
        }
        else if (activePlayer.Selection.Count > 0 && activePlayer.Selection.First() != null)
        {
            panel.style.display = DisplayStyle.Flex;
        }
        else
        {
            panel.style.display = DisplayStyle.None;
        }
    }

    [SerializeField]
    private VisualTreeAsset templateButton;

    private VisualElement panel;
}
