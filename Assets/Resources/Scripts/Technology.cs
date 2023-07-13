using System.Collections.Generic;

public class Technology
{
    public Technology(string name)
    {
        Name = name;
    }

    public Technology(string name, Dictionary<string, int> cost, bool unlocked)
    {
        Name = name;
        Cost = cost;
        Unlocked = unlocked;
    }

    public Technology(string name, Dictionary<string, int> cost, HashSet<string> unlocks)
    {
        Name = name;
        Cost = cost;
        Unlocks = unlocks;
    }

    public string Name { get; }

    public Dictionary<string, int> Cost { get; } = new Dictionary<string, int>();

    public bool Unlocked { get; set; } = false;

    public HashSet<string> Unlocks { get; set; } = new HashSet<string>();
}
