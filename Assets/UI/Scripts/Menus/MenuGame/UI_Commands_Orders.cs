using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Commands_Orders : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Commands_Orders");

        orders = panel.Q<VisualElement>("List_Orders");
    }

    private void Start()
    {
        CreateOrders();
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

                UpdateOrders(hovered);
            }
        }
        else if (activePlayer.Selection.Count > 0 && activePlayer.Selection.First() != null)
        {
            panel.style.display = DisplayStyle.Flex;

            UpdateOrders(null);
        }
        else
        {
            panel.style.display = DisplayStyle.None;
        }
    }

    private void CreateOrders()
    {
        List<OrderType> forbiddenInUI = new List<OrderType>()
        {
            OrderType.Assemble,
            OrderType.Construct,
            OrderType.GatherResource,
            OrderType.Idle,
            OrderType.Load,
            OrderType.None,
            OrderType.Produce,
            OrderType.Transport,
            OrderType.Unload,
            OrderType.UseSkill,
        };

        orders.Clear();

        foreach (string i in Utils.GetOrderNames())
        {
            if (forbiddenInUI.Contains(Enum.Parse<OrderType>(i)))
            {
                continue;
            }

            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnOrder(Enum.Parse<OrderType>(i)));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(i);

            orders.Add(buttonContainer);
            ordersButtons[Enum.Parse<OrderType>(i)] = button;
        }
    }

    private void UpdateOrders(MyGameObject hovered)
    {
        HashSet<OrderType> whitelist = new HashSet<OrderType>();

        if (hovered != null)
        {
            if (hovered.State == MyGameObjectState.Operational)
            {
                whitelist = new HashSet<OrderType>(hovered.Orders.OrderWhitelist);
            }
        }
        else
        {
            foreach (MyGameObject selected in HUD.Instance.ActivePlayer.Selection.Items)
            {
                if (selected.State == MyGameObjectState.Operational)
                {
                    whitelist.UnionWith(selected.Orders.OrderWhitelist);
                }
            }
        }

        foreach (Button button in ordersButtons.Values)
        {
            button.style.display = DisplayStyle.None;
        }

        foreach (OrderType i in whitelist)
        {
            if (ordersButtons.TryGetValue(i, out Button button))
            {
                button.style.display = DisplayStyle.Flex;
            }
        }
    }

    private void OnOrder(OrderType orderType)
    {
        Player activePlayer = HUD.Instance.ActivePlayer;

        switch (orderType)
        {
            case OrderType.Destroy:
                activePlayer.Selection.Destroy(MyInput.GetShift());
                break;

            case OrderType.Disable:
                activePlayer.Selection.Disable(MyInput.GetShift());
                break;

            case OrderType.Enable:
                activePlayer.Selection.Enable(MyInput.GetShift());
                break;

            case OrderType.Explore:
                activePlayer.Selection.Explore(MyInput.GetShift());
                break;

            case OrderType.Stop:
                activePlayer.Selection.Stop(MyInput.GetShift());
                break;

            case OrderType.Wait:
                activePlayer.Selection.Wait(MyInput.GetShift());
                break;

            default:
                HUD.Instance.Order = orderType;
                break;
        }
    }

    [SerializeField]
    private VisualTreeAsset templateButton;

    private VisualElement panel;

    private VisualElement orders;

    private Dictionary<OrderType, Button> ordersButtons = new Dictionary<OrderType, Button>();
}
