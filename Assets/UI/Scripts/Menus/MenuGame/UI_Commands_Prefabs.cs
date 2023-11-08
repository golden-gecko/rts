using System.Collections.Generic;
using System.IO;
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

    private void CreatePrefabs()
    {
        prefabs.Clear();

        foreach (GameObject gameObject in Config.Instance.Structures)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnConstruct(gameObject.name));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(gameObject.name);
            button.userData = gameObject.name;

            prefabs.Add(buttonContainer);
            prefabsButtons[gameObject.name] = button;
        }

        foreach (GameObject gameObject in Config.Instance.Units)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnAssemble(gameObject.name));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(Path.GetFileName(gameObject.name));
            button.userData = gameObject.name;

            prefabs.Add(buttonContainer);
            prefabsButtons[gameObject.name] = button;
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
                bool enabled = technologyTree.IsDiscovered(Path.GetFileName(i));

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
