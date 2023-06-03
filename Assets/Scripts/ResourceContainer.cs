using System.Collections.Generic;
using UnityEngine;

public class ResourceContainer
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

    public void Remove(string name, int value)
    {
        if (Items.ContainsKey(name) == false)
        {
            Items.Add(name, new Resource(name, 0, 0));
        }

        Items[name].Remove(value);
    }

    public Dictionary<string, Resource> Items { get; }
}
