using System.Collections.Generic;
using UnityEngine;

public class BlueprintManager : Singleton<BlueprintManager>
{
    public Blueprint Get(string name)
    {
        if (Blueprints.TryGetValue(name, out Blueprint blueprint))
        {
            return blueprint;
        }

        return null;
    }

    public void Save(string name, List<BlueprintComponent> parts)
    {
        Blueprints[name] = new Blueprint(name, parts);
    }

    private Dictionary<string, Blueprint> Blueprints = new Dictionary<string, Blueprint>();
}
