using System.Collections.Generic;

public class Technology
{
    public Technology(string name)
    {
        Name = name;
    }

    public Technology(string name, HashSet<string> unlocks)
    {
        Name = name;
        Unlocks = unlocks;
    }

    public string Name { get; }

    public ResourceContainer Cost { get; } = new ResourceContainer();

    public int MaxSum { get => Cost.MaxSum; }

    public bool Unlocked { get; set; } = false;

    public HashSet<string> Unlocks { get; } = new HashSet<string>();

    public bool Discovered { get; set; } = false;
}
