using System;
using System.Collections.Generic;

[Serializable]
public class Blueprint : ICloneable
{
    public object Clone()
    {
        return new Blueprint(Name, new List<BlueprintComponent>(Parts));
    }

    public Blueprint()
    {
    }

    public Blueprint(string name, List<BlueprintComponent> parts)
    {
        Name = name;
        Parts = parts;
    }

    public string Name;
    public List<BlueprintComponent> Parts = new List<BlueprintComponent>();
}
