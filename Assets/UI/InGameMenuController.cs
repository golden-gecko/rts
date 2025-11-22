using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameMenuController : MonoBehaviour
{
    void Start()
    {
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>();

        var uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;

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

        foreach (var i in AssetDatabase.GetAllAssetPaths())
        {
            if (i.Contains("Assets/Resources/") == false)
            {
                continue;
            }

            if (i.Contains(".prefab") == false)
            {
                continue;
            }


            var buttonContainer = templateButton.Instantiate();
            var button = buttonContainer.Q<Button>();
            var prefabPath = i.Replace("Assets/Resources/", "").Replace(".prefab", "");
            var resource = Resources.Load<MyGameObject>(prefabPath);

            button.RegisterCallback<ClickEvent>(ev => OnConstruct(prefabPath));
            button.style.display = DisplayStyle.None;
            button.text = Path.GetFileName(i).Replace(".prefab", "");
            button.userData = prefabPath;

            prefabs.Add(buttonContainer);
            prefabsButtons[prefabPath] = button;
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
        var whitelist = new HashSet<string>();

        foreach (var i in hud.Selected)
        {
            whitelist.UnionWith(i.Orders.PrefabWhitelist);
        }

        foreach (var i in prefabsButtons)
        {
            i.Value.style.display = DisplayStyle.None;
        }

        foreach (var i in whitelist)
        {
            if (prefabsButtons.ContainsKey(i))
            {
                prefabsButtons[i].style.display = DisplayStyle.Flex;
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

    Label order;
    Label prefab;
    Label selected;
    Label info;

    VisualElement orders;
    VisualElement prefabs;

    Dictionary<OrderType, Button> ordersButtons;
    Dictionary<string, Button> prefabsButtons;

}
