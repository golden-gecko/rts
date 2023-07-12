using System.Collections.Generic;

public class Technology
{
    public Technology(string name, bool unlocked)
    {
        Name = name;
        Unlocked = unlocked;
    }

    public Technology(string name, Dictionary<string, int> cost, bool unlocked)
    {
        Name = name;
        Cost = cost;
        Unlocked = unlocked;
    }

    public Technology(string name, Dictionary<string, int> cost, bool unlocked, List<string> unlocks)
    {
        Name = name;
        Cost = cost;
        Unlocked = unlocked;
        Unlocks = unlocks;
    }

    public string Name { get; }

    public Dictionary<string, int> Cost { get; } = new Dictionary<string, int>();

    public bool Unlocked { get; set; } = false;

    public List<string> Unlocks { get; } = new List<string>();
}
