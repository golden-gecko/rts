using System.Collections.Generic;

public class Technology
{
    public Technology(string name)
    {
        Name = name;
    }

    public Technology(string name, HashSet<string> requirements)
    {
        Name = name;
        Requirements = requirements;
    }

    public string Name { get; }

    public ResourceContainer Cost { get; } = new ResourceContainer();

    public int MaxSum { get => Cost.MaxSum; }

    public HashSet<string> Requirements { get; } = new HashSet<string>();

    public bool Discovered { get; set; } = false;
}
