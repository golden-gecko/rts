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

    void Start()
    {
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>();

        var uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;

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

    void Update()
    {
        order.text = "Order: " + hud.Order.ToString();
        prefab.text = "Prefab: " + hud.Prefab;
        selected.text = "Selected: " + hud.Selected.Count;

        if (hud.Selected.Count > 0)
        {
            info.text = hud.Selected[0].GetInfo();
        }
        else
        {
            info.text = string.Empty;
        }

        UpdateOrders();
        UpdatePrefabs();
    }

    void CreateOrders()
    {
        var forbiddenInUI = new List<OrderType>()
        {
            OrderType.Construct,
            OrderType.Destroy,
            OrderType.Idle,
            OrderType.None,
            OrderType.Produce,
        };

        orders.Clear();

        foreach (var i in Enum.GetNames(typeof(OrderType)))
        {
            if (forbiddenInUI.Contains(Enum.Parse<OrderType>(i)))
            {
                continue;
            }

            var buttonContainer = templateButton.Instantiate();
            var button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnOrder(Enum.Parse<OrderType>(i)));
            button.style.display = DisplayStyle.None;
            button.text = i;
            button.userData = Enum.Parse<OrderType>(i);

            orders.Add(buttonContainer);
            ordersButtons[Enum.Parse<OrderType>(i)] = button;
        }
    }

    void CreatePrefabs()
    {
        prefabs.Clear();

        // TODO: AssetDatabase does not work outside editor.
        var prefabList = new List<string>()
        {
            "Prefabs/Buildings/struct_Barracks_A_yup",
            "Prefabs/Buildings/struct_Factory_Heavy_A_yup",
            "Prefabs/Buildings/struct_Factory_Light_A_yup",
            "Prefabs/Buildings/struct_Headquarters_A_yup",
            "Prefabs/Buildings/struct_Misc_Building_B_yup",
            "Prefabs/Buildings/struct_Radar_Outpost_A_yup",
            "Prefabs/Buildings/struct_Refinery_A_yup",
            "Prefabs/Buildings/struct_Research_Lab_A_yup",
            "Prefabs/Buildings/struct_Spaceport_A_yup",
            "Prefabs/Buildings/struct_Turret_Gun_A_yup",
            "Prefabs/Buildings/struct_Turret_Missile_A_yup",
            "Prefabs/Buildings/struct_Wall_A_yup",
            "Prefabs/Vehicles/unit_Grav_Light_A_yup",
            "Prefabs/Vehicles/unit_Harvester_A_yup",
            "Prefabs/Vehicles/unit_Infantry_Light_B_yup",
            "Prefabs/Vehicles/unit_Quad_A_yup",
            "Prefabs/Vehicles/unit_Tank_Combat_A_yup",
            "Prefabs/Vehicles/unit_Tank_Missile_A_yup",
            "Prefabs/Vehicles/unit_Trike_A_yup",
        };

        foreach (var i in prefabList)
        {
            var buttonContainer = templateButton.Instantiate();
            var button = buttonContainer.Q<Button>();
            var resource = Resources.Load<MyGameObject>(i);

            button.RegisterCallback<ClickEvent>(ev => OnConstruct(i));
            button.style.display = DisplayStyle.None;
            button.text = Path.GetFileName(i).Replace("struct_", "").Replace("unit_", "").Replace("_A_yup", "").Replace("_B_yup", "").Replace("_", " ");
            button.userData = i;

            prefabs.Add(buttonContainer);
            prefabsButtons[i] = button;
        }
    }

    void UpdateOrders()
    {
        var whitelist = new HashSet<OrderType>();

        foreach (var i in hud.Selected)
        {
            whitelist.UnionWith(i.Orders.OrderWhitelist);
        }

        foreach (var i in ordersButtons)
        {
            i.Value.style.display = DisplayStyle.None;
        }

        foreach (var i in whitelist)
        {
            if (ordersButtons.ContainsKey(i))
            {
                ordersButtons[i].style.display = DisplayStyle.Flex;
            }
        }
    }

    void UpdatePrefabs()
    {
        var whitelist = new Dictionary<string, PrefabConstructionType>();

        foreach (var selected in hud.Selected)
        {
            foreach (var prefab in selected.Orders.PrefabWhitelist)
            {
                whitelist[prefab.Key] = prefab.Value;
            }
        }

        foreach (var i in prefabsButtons)
        {
            i.Value.style.display = DisplayStyle.None;
        }

        foreach (var i in whitelist)
        {
            if (prefabsButtons.ContainsKey(i.Key))
            {
                prefabsButtons[i.Key].style.display = DisplayStyle.Flex;
            }
        }
    }

    void OnConstruct(string prefab)
    {
        hud.Order = OrderType.Construct;
        hud.Prefab = prefab;
    }

    void OnOrder(OrderType orderType)
    {
        hud.Order = orderType;
        hud.Prefab = string.Empty;
    }

    public VisualTreeAsset templateButton;
    
    HUD hud;

    Label log;

    Label order;
    Label prefab;
    Label selected;
    Label info;

    VisualElement orders;
    VisualElement prefabs;

    Dictionary<OrderType, Button> ordersButtons;
    Dictionary<string, Button> prefabsButtons;

}
