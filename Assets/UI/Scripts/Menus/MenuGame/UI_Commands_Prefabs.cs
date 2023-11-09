using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Commands_Prefabs : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Commands_Prefabs");

        prefabs = panel.Q<VisualElement>("List_Prefabs");
    }

    private void Start()
    {
        CreatePrefabs();
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

                UpdatePrefabs(hovered);
            }
        }
        else if (activePlayer.Selection.Count > 0 && activePlayer.Selection.First() != null)
        {
            panel.style.display = DisplayStyle.Flex;

            UpdatePrefabs(null);
        }
        else
        {
            panel.style.display = DisplayStyle.None;
        }
    }

    public void Refresh()
    {
        CreatePrefabs();
    }

    private void CreatePrefabs()
    {
        prefabs.Clear();

        foreach (GameObject structure in Game.Instance.Config.Structures)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnConstruct(structure.name));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(structure.name);
            button.userData = structure.name;

            prefabs.Add(buttonContainer);
            prefabsButtons[structure.name] = button;
        }

        foreach (GameObject unit in Game.Instance.Config.Units)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnAssemble(unit.name));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(Path.GetFileName(unit.name));
            button.userData = unit.name;

            prefabs.Add(buttonContainer);
            prefabsButtons[unit.name] = button;
        }

        foreach (string blueprint in Game.Instance.BlueprintManager.Blueprints.Keys)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnAssemble(blueprint));
            button.style.display = DisplayStyle.None;
            button.text = string.Format("{0} (BP)", Utils.FormatName(Path.GetFileName(blueprint)));
            button.userData = blueprint;

            prefabs.Add(buttonContainer);
            prefabsButtons[blueprint] = button;
        }
    }

    private void UpdatePrefabs(MyGameObject hovered)
    {
        HashSet<string> whitelist = new HashSet<string>();

        if (hovered != null)
        {
            if (hovered.State == MyGameObjectState.Operational)
            {
                whitelist = new HashSet<string>(hovered.Orders.PrefabWhitelist);
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

                foreach (string prefab in selected.Orders.PrefabWhitelist)
                {
                    whitelist.Add(prefab);
                }
            }
        }

        foreach (Button button in prefabsButtons.Values)
        {
            button.style.display = DisplayStyle.None;
        }

        TechnologyTree technologyTree = HUD.Instance.ActivePlayer.TechnologyTree;

        foreach (string i in whitelist)
        {
            if (prefabsButtons.TryGetValue(i, out Button button))
            {
                bool enabled = technologyTree.IsDiscovered(i) || Game.Instance.BlueprintManager.Blueprints.Keys.Contains(i);

                button.style.display = DisplayStyle.Flex;
                button.SetEnabled(enabled);
            }
        }
    }

    private void OnAssemble(string prefab)
    {
        HUD.Instance.ActivePlayer.Selection.Assemble(prefab, MyInput.GetShift());
    }

    private void OnConstruct(string prefab)
    {
        HUD.Instance.Order = OrderType.Construct;
        HUD.Instance.Prefab = prefab;
    }

    [SerializeField]
    private VisualTreeAsset templateButton;

    private VisualElement panel;

    private VisualElement prefabs;

    private Dictionary<string, Button> prefabsButtons = new Dictionary<string, Button>();
}
