using System.Collections.Generic;

public class OrderContainer
{
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
            info += string.Format("\n  {0}", order.GetInfo());
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

    public int Count { get => Items.Count; }

    public List<Order> Items { get; } = new();

    public HashSet<OrderType> OrderWhitelist { get; } = new();

    public HashSet<string> PrefabWhitelist { get; } = new();
}
