using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceContainer : IEnumerable<KeyValuePair<string, Resource>>
{
    public ResourceContainer()
    {
        Items = new Dictionary<string, Resource>();
    }

    public void Add(string name, int value)
    {
        if (Items.ContainsKey(name) == false)
        {
            Items.Add(name, new Resource(name, 0, 0));
        }

        Items[name].Add(value);
    }

    public void Add(string name, int value, int max)
    {
        if (Items.ContainsKey(name) == false)
        {
            Items.Add(name, new Resource(name, 0, 0));
        }

        // TODO: Order of initialization.
        Items[name].Max = max;
        Items[name].Add(value);
    }

    public bool CanAdd(string name, int value)
    {
        if (Items.ContainsKey(name) == false)
        {
            Items.Add(name, new Resource(name, 0, 0));
        }

        return Items[name].CanAdd(value);
    }

    public bool CanRemove(string name, int value)
    {
        if (Items.ContainsKey(name) == false)
        {
            Items.Add(name, new Resource(name, 0, 0));
        }

        return Items[name].CanRemove(value);
    }

    public int Capacity(string name)
    {
        if (Items.ContainsKey(name) == false)
        {
            Items.Add(name, new Resource(name, 0, 0));
        }

        return Items[name].Capacity();
    }

    public IEnumerator<KeyValuePair<string, Resource>> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    public string GetInfo()
    {
        var info = string.Empty;

        foreach (var i in Items)
        {
            info += string.Format("\n  {0} {1}", i.Key, i.Value.Value);
        }

        return info;
    }

    public void Remove(string name, int value)
    {
        if (Items.ContainsKey(name) == false)
        {
            Items.Add(name, new Resource(name, 0, 0));
        }

        Items[name].Remove(value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Dictionary<string, Resource> Items { get; }
}
