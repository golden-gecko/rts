using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ResourceContainer
{
    public void Add(string name, int value)
    {
        Resource resource = Items.Find(x => x.Name == name);

        if (resource != null)
        {
            resource.Add(value);
        }
        else
        {
            Items.Add(new Resource(name, value));
        }
    }

    public void Add(string name, int value, int max) // TODO: Max is ignored on second call.
    {
        Resource resource = Items.Find(x => x.Name == name);

        if (resource != null)
        {
            resource.Add(value);
        }
        else
        {
            Items.Add(new Resource(name, value, max));
        }
    }

    public void Add(string name, int value, int max, ResourceDirection direction) // TODO: Max is ignored on second call.
    {
        Resource resource = Items.Find(x => x.Name == name);

        if (resource != null)
        {
            resource.Add(value);
        }
        else
        {
            Items.Add(new Resource(name, value, max, direction));
        }
    }

    public void Remove(string name, int value)
    {
        Resource resource = Items.Find(x => x.Name == name);

        if (resource != null)
        {
            resource.Remove(value);
        }
    }

    public bool CanAdd(string name, int value)
    {
        Resource resource = Items.Find(x => x.Name == name);

        return resource != null && resource.CanAdd(value);
    }

    public bool CanRemove(string name, int value)
    {
        Resource resource = Items.Find(x => x.Name == name);

        return resource != null && resource.CanRemove(value);
    }

    public int Capacity(string name)
    {
        Resource resource = Items.Find(x => x.Name == name);

        return resource != null ? resource.Capacity : 0;
    }

    public int Storage(string name)
    {
        Resource resource = Items.Find(x => x.Name == name);

        return resource != null ? resource.Storage : 0;
    }

    public string GetInfo()
    {
        string info = string.Empty;

        foreach (Resource i in Items)
        {
            if (i.Max > 0)
            {
                info += string.Format("\n  {0} {1}", i.Name, i.GetInfo());
            }
        }

        return info;
    }

    public int Sum
    {
        get
        {
            return Items.Sum(x => x.Storage);
        }
    }

    [field: SerializeField]
    public List<Resource> Items { get; set; } = new List<Resource>();
}
