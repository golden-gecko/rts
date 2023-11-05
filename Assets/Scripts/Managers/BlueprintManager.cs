using System.Collections.Generic;

public class BlueprintManager : Singleton<DiplomacyManager>
{
    public Blueprint Get(string name)
    {
        if (Blueprints.TryGetValue(name, out Blueprint blueprint))
        {
            return blueprint;
        }

        return null;
    }

    private Dictionary<string, Blueprint> Blueprints = new Dictionary<string, Blueprint>();
}
