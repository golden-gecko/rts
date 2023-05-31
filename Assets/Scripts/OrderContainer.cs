using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class OrderContainer
{
    public OrderContainer()
    {
        Items = new List<Order>();
        Whitelist = new HashSet<OrderType>();
    }

    public OrderContainer(HashSet<OrderType> whitelist)
    {
        Items = new List<Order>();
        Whitelist = whitelist;
    }

    public void Add(Order item)
    {
        Items.Add(item);
    }

    public void Allow(OrderType item)
    {
        Whitelist.Add(item);
    }

    public bool Contains(OrderType item)
    {
        return Whitelist.Contains(item);
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

    public void RemoveAt(int index)
    {
        Items.RemoveAt(index);
    }

    public List<Order> Items { get; }

    public HashSet<OrderType> Whitelist { get; }

    public int Count { get => Items.Count; }
}
