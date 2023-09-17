using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ResourceContainer
{
    public void Init(string name, int value = 0, int max = 0, ResourceDirection direction = ResourceDirection.None)
    {
        Resource resource = Items.Find(x => x.Name == name);

        if (resource == null)
        {
            Items.Add(new Resource(name, value, max, direction));
        }
    }

    public int Add(string name, int value)
    {
        Resource resource = Items.Find(x => x.Name == name);

        return resource != null ? resource.Add(value) : 0;
    }

    public int Remove(string name, int value)
    {
        Resource resource = Items.Find(x => x.Name == name);

        return resource != null ? resource.Remove(value) : 0;
    }

    public void Inc(string name)
    {
        Resource resource = Items.Find(x => x.Name == name);

        if (resource != null)
        {
            resource.Inc();
        }
    }

    public void Dec(string name)
    {
        Resource resource = Items.Find(x => x.Name == name);

        if (resource != null)
        {
            resource.Dec();
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

    public bool CanInc(string name)
    {
        Resource resource = Items.Find(x => x.Name == name);

        return resource != null && resource.CanAdd(1);
    }

    public bool CanDec(string name)
    {
        Resource resource = Items.Find(x => x.Name == name);

        return resource != null && resource.CanRemove(1);
    }

    public int Current(string name)
    {
        Resource resource = Items.Find(x => x.Name == name);

        return resource != null ? resource.Current : 0;
    }

    public int Max(string name)
    {
        Resource resource = Items.Find(x => x.Name == name);

        return resource != null ? resource.Max : 0;
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

    public float Percent(string name)
    {
        Resource resource = Items.Find(x => x.Name == name);

        return resource != null ? resource.Percent : 0.0f;
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

    public int StorageSum
    {
        get
        {
            return Items.Sum(x => x.Storage);
        }
    }

    public int MaxSum
    {
        get
        {
            return Items.Sum(x => x.Max);
        }
    }

    public bool In { get => Items.Any(x => x.In); }

    public bool Out { get => Items.Any(x => x.Out); }

    [field: SerializeField]
    public List<Resource> Items { get; set; } = new List<Resource>();

    [field: SerializeField] // TODO: Implement.
    public int MaxStorage { get; set; } = -1; // -1 for unlimited storage.
}
