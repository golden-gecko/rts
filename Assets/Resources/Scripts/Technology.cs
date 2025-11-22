using System.Collections.Generic;

public class Technology
{
    public Technology(string name)
    {
        Name = name;
    }

    public Technology(string name, ResourceContainer cost, bool unlocked)
    {
        Name = name;
        Cost = cost;
        Unlocked = unlocked;
    }

    public Technology(string name, ResourceContainer cost, HashSet<string> unlocks)
    {
        Name = name;
        Cost = cost;
        Unlocks = unlocks;
    }

    public string Name { get; }

    public ResourceContainer Cost { get; } = new ResourceContainer();

    public bool Researched
    {
        get
        {
            foreach (Resource resource in Cost.Items.Values)
            {
                if (resource.Capacity > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public bool Unlocked { get; set; } = false;

    public HashSet<string> Unlocks { get; set; } = new HashSet<string>();
}
