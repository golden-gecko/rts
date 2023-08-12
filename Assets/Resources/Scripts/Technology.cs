using System.Collections.Generic;

public class Technology
{
    public Technology(string name)
    {
        Name = name;
    }

    public Technology(string name, bool unlocked)
    {
        Name = name;
        Unlocked = unlocked;
    }

    public Technology(string name, HashSet<string> unlocks)
    {
        Name = name;
        Unlocks = unlocks;
    }

    public string Name { get; }

    public ResourceContainer Cost { get; } = new ResourceContainer();

    public int Total
    {
        get
        {
            int sum = 0;

            foreach (Resource i in Cost.Items.Values)
            {
                sum += i.Max;
            }

            return sum;
        }
    }

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
