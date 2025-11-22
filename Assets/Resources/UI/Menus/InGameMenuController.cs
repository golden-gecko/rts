using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameMenuController : MonoBehaviour
{
    public void Log(string message)
    {
        log.text = message;
    }

    private void CreateOrders()
    {
        List<OrderType> forbiddenInUI = new List<OrderType>()
        {
            OrderType.Assemble,
            OrderType.Construct,
            OrderType.Idle,
            OrderType.None,
        };

        orders.Clear();

        foreach (string i in Enum.GetNames(typeof(OrderType)))
        {
            if (forbiddenInUI.Contains(Enum.Parse<OrderType>(i)))
            {
                continue;
            }

            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnOrder(Enum.Parse<OrderType>(i)));
            button.style.display = DisplayStyle.None;
            button.text = i;
            button.userData = Enum.Parse<OrderType>(i);

            orders.Add(buttonContainer);
            ordersButtons[Enum.Parse<OrderType>(i)] = button;
        }
    }

    private void CreatePrefabs()
    {
        prefabs.Clear();

        Dictionary<string, PrefabConstructionType> prefabList = new Dictionary<string, PrefabConstructionType>() // TODO: AssetDatabase does not work outside editor.
        {
            { "Objects/Structures/struct_Barracks_A_yup", PrefabConstructionType.Structure },
            { "Objects/Structures/struct_Factory_Heavy_A_yup", PrefabConstructionType.Structure },
            { "Objects/Structures/struct_Factory_Light_A_yup", PrefabConstructionType.Structure },
            { "Objects/Structures/struct_Headquarters_A_yup", PrefabConstructionType.Structure },
            { "Objects/Structures/struct_Misc_Building_B_yup", PrefabConstructionType.Structure },
            { "Objects/Structures/struct_Radar_Outpost_A_yup", PrefabConstructionType.Structure },
            { "Objects/Structures/struct_Refinery_A_yup", PrefabConstructionType.Structure },
            { "Objects/Structures/struct_Research_Lab_A_yup", PrefabConstructionType.Structure },
            { "Objects/Structures/struct_Spaceport_A_yup", PrefabConstructionType.Structure },
            { "Objects/Structures/struct_Turret_Gun_A_yup", PrefabConstructionType.Structure },
            { "Objects/Structures/struct_Turret_Missile_A_yup", PrefabConstructionType.Structure },
            { "Objects/Structures/struct_Wall_A_yup", PrefabConstructionType.Structure },
            { "Objects/Units/unit_Grav_Light_A_yup", PrefabConstructionType.Unit },
            { "Objects/Units/unit_Harvester_A_yup", PrefabConstructionType.Unit },
            { "Objects/Units/unit_Infantry_Light_B_yup", PrefabConstructionType.Unit },
            { "Objects/Units/unit_Quad_A_yup", PrefabConstructionType.Unit },
            { "Objects/Units/unit_Tank_Combat_A_yup", PrefabConstructionType.Unit },
            { "Objects/Units/unit_Tank_Missile_A_yup", PrefabConstructionType.Unit },
            { "Objects/Units/unit_Trike_A_yup", PrefabConstructionType.Unit },
        };

        foreach (KeyValuePair<string, PrefabConstructionType> i in prefabList)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            switch (i.Value)
            {
                case PrefabConstructionType.Structure:
                    button.RegisterCallback<ClickEvent>(ev => OnConstruct(i.Key));
                    break;

                case PrefabConstructionType.Unit:
                    button.RegisterCallback<ClickEvent>(ev => OnAssemble(i.Key));
                    break;
            }

            button.style.display = DisplayStyle.None;
            button.text = Path.GetFileName(i.Key).Replace("struct_", "").Replace("unit_", "").Replace("_A_yup", "").Replace("_B_yup", "").Replace("_", " ");
            button.userData = i;

            prefabs.Add(buttonContainer);
            prefabsButtons[i.Key] = button;
        }
    }

    private void OnAssemble(string prefab)
    {
        hud.Assemble(prefab);
    }

    private void OnConstruct(string prefab)
    {
        hud.Order = OrderType.Construct;
        hud.Prefab = prefab;
    }

    private void OnOrder(OrderType orderType)
    {
        hud.Order = orderType;
        hud.Prefab = string.Empty;
    }

    private void Start()
    {
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>();

        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement rootVisualElement = uiDocument.rootVisualElement;

        bottomPanel = rootVisualElement.Q<VisualElement>("BottomPanel");
        infoPanel = rootVisualElement.Q<VisualElement>("InfoPanel");

        log = rootVisualElement.Q<Label>("Log");

        order = rootVisualElement.Q<Label>("Order");
        prefab = rootVisualElement.Q<Label>("Prefab");
        selected = rootVisualElement.Q<Label>("Selected");
        info = rootVisualElement.Q<Label>("Info");

        orders = rootVisualElement.Q<VisualElement>("OrderList");
        prefabs = rootVisualElement.Q<VisualElement>("PrefabList");

        ordersButtons = new Dictionary<OrderType, Button>();
        prefabsButtons = new Dictionary<string, Button>();

        CreateOrders();
        CreatePrefabs();

        Log("");
    }

    private void Update()
    {
        order.text = "Order: " + hud.Order.ToString();
        prefab.text = "Prefab: " + hud.Prefab;
        selected.text = "Selected: " + hud.Selected.Count;

        if (hud.Selected.Count > 0 && hud.Selected[0] != null)
        {
            info.text = hud.Selected[0].GetInfo();
        }
        else
        {
            info.text = string.Empty;
        }

        UpdateOrders();
        UpdatePrefabs();

        if (hud.Selected.Count > 0)
        {
            bottomPanel.style.display = DisplayStyle.Flex;
            infoPanel.style.display = DisplayStyle.Flex;
        }
        else
        {
            bottomPanel.style.display = DisplayStyle.None;
            infoPanel.style.display = DisplayStyle.None;
        }
    }

    private void UpdateOrders()
    {
        HashSet<OrderType> whitelist = new HashSet<OrderType>();

        foreach (MyGameObject selected in hud.Selected)
        {
            if (selected.State != MyGameObjectState.Operational)
            {
                continue;
            }

            whitelist.UnionWith(selected.Orders.OrderWhitelist);
        }

        foreach (KeyValuePair<OrderType, Button> button in ordersButtons)
        {
            button.Value.style.display = DisplayStyle.None;
        }

        foreach (OrderType i in whitelist)
        {
            if (ordersButtons.ContainsKey(i))
            {
                ordersButtons[i].style.display = DisplayStyle.Flex;
            }
        }
    }

    private void UpdatePrefabs()
    {
        HashSet<string> whitelist = new HashSet<string>();

        foreach (MyGameObject selected in hud.Selected)
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

        foreach (KeyValuePair<string, Button> button in prefabsButtons)
        {
            button.Value.style.display = DisplayStyle.None;
        }

        foreach (string i in whitelist)
        {
            if (prefabsButtons.ContainsKey(i))
            {
                prefabsButtons[i].style.display = DisplayStyle.Flex;
            }
        }
    }

    public VisualTreeAsset templateButton;

    private HUD hud;

    private VisualElement bottomPanel;
    private VisualElement infoPanel;

    private Label log;

    private Label order;
    private Label prefab;
    private Label selected;
    private Label info;

    private VisualElement orders;
    private VisualElement prefabs;

    private Dictionary<OrderType, Button> ordersButtons;
    private Dictionary<string, Button> prefabsButtons;
}
