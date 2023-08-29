using System.Collections.Generic;

public class ResourceContainer
{
    public void Add(string name, int value)
    {
        if (Items.ContainsKey(name))
        {
            Items[name].Add(value);
        }
        else
        {
            Items.Add(name, new Resource(name, value));
        }
    }

    public void Add(string name, int value, int max) // TODO: Max is ignored on second call.
    {
        if (Items.ContainsKey(name))
        {
            Items[name].Add(value);
        }
        else
        {
            Items.Add(name, new Resource(name, value, max));
        }
    }

    public void Add(string name, int value, int max, ResourceDirection direction) // TODO: Max is ignored on second call.
    {
        if (Items.ContainsKey(name))
        {
            Items[name].Add(value);
        }
        else
        {
            Items.Add(name, new Resource(name, value, max, direction));
        }
    }

    public void Remove(string name, int value)
    {
        if (Items.ContainsKey(name))
        {
            Items[name].Remove(value);
        }
    }

    public bool CanAdd(string name, int value)
    {
        return Items.ContainsKey(name) && Items[name].CanAdd(value);
    }

    public bool CanRemove(string name, int value)
    {
        return Items.ContainsKey(name) && Items[name].CanRemove(value);
    }

    public int Capacity(string name)
    {
        return Items.ContainsKey(name) ? Items[name].Capacity : 0;
    }

    public int Storage(string name)
    {
        return Items.ContainsKey(name) ? Items[name].Storage : 0;
    }

    public string GetInfo()
    {
        string info = string.Empty;

        foreach (KeyValuePair<string, Resource> i in Items)
        {
            if (i.Value.Max > 0)
            {
                info += string.Format("\n  {0} {1}", i.Key, i.Value.GetInfo());
            }
        }

        return info;
    }

    public Dictionary<string, Resource> Items { get; } = new Dictionary<string, Resource>();
}
