using System.Collections.Generic;

public class Technology
{
    public Technology(string name, bool unlockable, bool unlocked)
    {
        Name = name;
        Unlockable = unlockable;
        Unlocked = unlocked;
    }

    public Technology(string name, bool unlockable, bool unlocked, List<string> unlocks)
    {
        Name = name;
        Unlockable = unlockable;
        Unlocked = unlocked;
        Unlocks = unlocks;
    }

    public string Name { get; }

    public bool Unlockable { get; } = false;

    public bool Unlocked { get; set; } = false;

    public List<string> Unlocks { get; } = new List<string>();
}
