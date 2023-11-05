using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
    public Blueprint(string name)
    {
        Name = name;
    }

    public void Attach(PartType partType, Part part, Vector3 position)
    {
        Dettach(partType);

        Parts.Add(new BlueprintComponent { PartType = partType, Part = part, Position = position });
    }

    public void Dettach(PartType partType)
    {
        Parts.RemoveAll(x => x.PartType == partType);
    }

    public void Create(Vector3 position)
    {
        foreach (BlueprintComponent i in Parts)
        {
        }
    }

    public string Name { get; }

    public List<BlueprintComponent> Parts { get; } = new List<BlueprintComponent>();
}
