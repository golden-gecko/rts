using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ResourceContainer
{
    public void Init(string name, int current = 0, int max = 0, ResourceDirection direction = ResourceDirection.None)
    {
        Resource resource = Items.Find(x => x.Name == name);

        if (resource == null)
        {
            Items.Add(new Resource(name, current, max, direction));
        }
    }

    public void Update(string name, int current = 0, int max = 0, ResourceDirection direction = ResourceDirection.None)
    {
        Resource resource = Items.Find(x => x.Name == name);

        if (resource == null)
        {
            Items.Add(new Resource(name, current, max, direction));
        }
        else
        {
            resource.Current = current;
            resource.Max = max;
            resource.Direction = direction;
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

    public void RemoveAll()
    {
        Items.ForEach(x => x.RemoveAll());
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

    public int Available(string name)
    {
        Resource resource = Items.Find(x => x.Name == name);

        return resource != null ? resource.Available : 0;
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

    public void Request(Recipe recipe)
    {
        foreach (Resource resource in Items)
        {
            resource.Direction = ResourceDirection.Out;
        }

        foreach (Resource resource in recipe.ToConsume.Items)
        {
            Update(resource.Name, 0, resource.Max, ResourceDirection.In); // TODO: Add buffer to Max.
        }

        foreach (Resource resource in recipe.ToProduce.Items)
        {
            Update(resource.Name, 0, resource.Max, ResourceDirection.Out); // TODO: Add buffer to Max.
        }
    }

    [field: SerializeField]
    public List<Resource> Items { get; set; } = new List<Resource>();

    public int CurrentSum { get => Items.Sum(x => x.Current); }

    public int MaxSum { get => Items.Sum(x => x.Max); }

    public bool Empty { get => Items.All(x => x.Empty); }

    public bool Full { get => Items.All(x => x.Full); }

    public float PercentSum { get => (float)CurrentSum / (float)MaxSum; }

    public bool In { get => Items.Any(x => x.In); }

    public bool Out { get => Items.Any(x => x.Out); }
}
