using System.Collections.Generic;
using System.Linq;

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
        Items.Add(item);
    }

    public void AllowOrder(OrderType item)
    {
        OrderWhitelist.Add(item);
    }

    public void AllowPrefab(string item)
    {
        PrefabWhitelist.Add(item);
    }

    public bool Contains(OrderType item)
    {
        return OrderWhitelist.Contains(item);
    }

    public void Clear()
    {
        Items.Clear();
    }

    public Order First()
    {
        return Items.First();
    }

    public void Insert(int index, Order item)
    {
        Items.Insert(index, item);
    }

    public void MoveToEnd()
    {
        Add(Items.First());
        Pop();
    }

    public string GetInfo()
    {
        var info = string.Empty;

        foreach (var i in Items)
        {
            info += i.GetInfo() + ", ";
        }

        if (info.Length > 2)
        {
            info = info.Substring(0, info.Length - 2);
        }

        return info;
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
