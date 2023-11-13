using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Commands_Technologies : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Commands_Technologies");

        technologies = panel.Q<VisualElement>("List_Technologies");
    }

    private void Start()
    {
        CreateTechnologies();
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

                UpdateTechnologies(hovered);
            }
        }
        else if (activePlayer.Selection.Count > 0 && activePlayer.Selection.First() != null)
        {
            panel.style.display = DisplayStyle.Flex;

            UpdateTechnologies(null);
        }
        else
        {
            panel.style.display = DisplayStyle.None;
        }
    }

    private void CreateTechnologies()
    {
        technologies.Clear();

        foreach (string i in HUD.Instance.ActivePlayer.TechnologyTree.Technologies.Keys)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnResearch(i));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(i);

            technologies.Add(buttonContainer);
            technologiesButtons[i] = button;
        }
    }

    private void UpdateTechnologies(MyGameObject hovered)
    {
        HashSet<string> whitelist = new HashSet<string>();

        if (hovered != null)
        {
            if (hovered.State == MyGameObjectState.Operational)
            {
                whitelist = new HashSet<string>(hovered.Orders.TechnologyWhitelist);
            }
        }
        else
        {
            foreach (MyGameObject selected in HUD.Instance.ActivePlayer.Selection.Items)
            {
                if (selected.State != MyGameObjectState.Operational)
                {
                    continue;
                }

                foreach (string prefab in selected.Orders.TechnologyWhitelist)
                {
                    whitelist.Add(prefab);
                }
            }
        }

        foreach (Button button in technologiesButtons.Values)
        {
            button.style.display = DisplayStyle.None;
        }

        TechnologyTree technologyTree = HUD.Instance.ActivePlayer.TechnologyTree;

        foreach (string i in whitelist)
        {
            if (technologiesButtons.TryGetValue(i, out Button button))
            {
                bool enabled = technologyTree.IsReadyToDiscover(i);

                button.style.display = DisplayStyle.Flex;
                button.SetEnabled(enabled);
            }
        }
    }

    private void OnResearch(string technology)
    {
        HUD.Instance.ActivePlayer.Selection.Research(technology, MyInput.GetShift());
    }

    [SerializeField]
    private VisualTreeAsset templateButton;

    private VisualElement panel;

    private VisualElement technologies;

    private Dictionary<string, Button> technologiesButtons = new Dictionary<string, Button>();
}
