using System.Collections.Generic;

public class OrderContainer
{
    public OrderContainer()
    {
        Items = new List<Order>();
        OrderWhitelist = new HashSet<OrderType>();
        PrefabWhitelist = new HashSet<string>();
    }

    public void Add(Order item)
    {
        if (OrderWhitelist.Contains(item.Type))
        {
            Items.Add(item);
        }
    }

    public void AllowOrder(OrderType item)
    {
        OrderWhitelist.Add(item);
    }

    public void AllowPrefab(string item)
    {
        PrefabWhitelist.Add(item);
    }

    public void Clear()
    {
        Items.Clear();
    }

    public Order First()
    {
        return Items[0];
    }

    public string GetInfo()
    {
        string info = string.Empty;

        foreach (Order order in Items)
        {
            info += order.GetInfo() + ", ";
        }

        if (info.Length > 2)
        {
            info = info.Substring(0, info.Length - 2);
        }

        return info;
    }

    public void Insert(int index, Order item)
    {
        Items.Insert(index, item);
    }

    public bool IsAllowed(OrderType item)
    {
        return OrderWhitelist.Contains(item);
    }

    public void MoveToEnd()
    {
        Add(Items[0]);
        Pop();
    }

    public void Pop()
    {
        Items.RemoveAt(0);
    }

    public List<Order> Items { get; }

    public HashSet<OrderType> OrderWhitelist { get; }

    public HashSet<string> PrefabWhitelist { get; }

    public int Count { get => Items.Count; }
}
