using System.Collections.Generic;

public class Technology
{
    public Technology(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public Technology(string name, string description, bool unlocked)
    {
        Name = name;
        Description = description;
        Unlocked = unlocked;
    }

    public Technology(string name, string description, HashSet<string> unlocks)
    {
        Name = name;
        Description = description;
        Unlocks = unlocks;
    }

    public string Name { get; }

    public string Description { get; }

    public ResourceContainer Cost { get; } = new ResourceContainer();

    public int MaxSum { get => Cost.MaxSum; }

    public bool Researched
    {
        get
        {
            foreach (Resource resource in Cost.Items)
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

    public HashSet<string> Unlocks { get; } = new HashSet<string>();
}
