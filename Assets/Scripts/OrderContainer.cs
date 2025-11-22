using System.Collections;
using System.Collections.Generic;

public enum PrefabConstructionType
{
    Structure,
    Unit,
}

public class OrderContainer : IEnumerable<Order>
{
    public OrderContainer()
    {
        Items = new List<Order>();
        OrderWhitelist = new HashSet<OrderType>();
        PrefabWhitelist = new Dictionary<string, PrefabConstructionType>();
    }

    public void Add(Order item)
    {
        Items.Add(item);
    }

    public void AllowOrder(OrderType item)
    {
        OrderWhitelist.Add(item);
    }

    public void AllowPrefab(string item, PrefabConstructionType prefabConstructionType)
    {
        PrefabWhitelist[item] = prefabConstructionType;
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
        return Items[0];
    }

    public void Insert(int index, Order item)
    {
        Items.Insert(index, item);
    }

    public void MoveToEnd()
    {
        Add(Items[0]);
        Pop();
    }

    public IEnumerator<Order> GetEnumerator()
    {
        return Items.GetEnumerator();
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

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public List<Order> Items { get; }

    public HashSet<OrderType> OrderWhitelist { get; }

    public Dictionary<string, PrefabConstructionType> PrefabWhitelist { get; }

    public int Count { get => Items.Count; }
}
