using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class ResourceContainer
{
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

        return Items[name].Capacity;
    }

    public Dictionary<string, int> GetCapacity()
    {
        Dictionary<string, int> capacity = new Dictionary<string, int>();

        foreach (Resource resource in Items.Values)
        {
            capacity[resource.Name] = resource.Capacity;
        }

        return capacity;
    }

    public Dictionary<string, int> GetStorage()
    {
        Dictionary<string, int> storage = new Dictionary<string, int>();

        foreach (Resource resource in Items.Values)
        {
            storage[resource.Name] = resource.Storage;
        }

        return storage;
    }

    public string GetInfo()
    {
        string info = string.Empty;

        foreach (KeyValuePair<string, Resource> i in Items)
        {
            if (i.Value.Current > 0)
            {
                info += string.Format("\n  {0} {1}/{2}", i.Key, i.Value.Current, i.Value.Max);
            }
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

    public int Storage(string name)
    {
        if (Items.ContainsKey(name) == false)
        {
            Items.Add(name, new Resource(name, 0, 0));
        }

        return Items[name].Storage;
    }

    public Dictionary<string, Resource> Items { get; private set; } = new Dictionary<string, Resource>();
}
