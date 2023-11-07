using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BlueprintManager : Singleton<BlueprintManager>
{
    protected override void Awake()
    {
        base.Awake();

        LoadBlueprints();
    }

    public Blueprint Get(string name)
    {
        if (Blueprints.TryGetValue(name, out Blueprint blueprint))
        {
            return blueprint;
        }

        return null;
    }

    public void Save(Blueprint blueprint)
    {
        Blueprints[blueprint.Name] = blueprint;
    }

    private void LoadBlueprints()
    {
        foreach (string file in Directory.EnumerateFiles(Config.Blueprints.Directory))
        {
            if (Path.GetExtension(file).ToLower() != ".json")
            {
                continue;
            }

            string json = File.ReadAllText(file);
            Blueprint blueprint = JsonUtility.FromJson<Blueprint>(json);

            Blueprints[blueprint.Name] = blueprint;
        }
    }

    public Dictionary<string, Blueprint> Blueprints = new Dictionary<string, Blueprint>();
}
