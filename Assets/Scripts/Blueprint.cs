using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    public void Save()
    {
        string directory = Config.Blueprints.Directory;
        string path = Path.Join(directory, string.Format("{0}.json", Name));
        string json = JsonUtility.ToJson(this, true);

        Directory.CreateDirectory(directory);
        File.WriteAllText(path, json);
    }

    public void Delete()
    {
        File.Delete(Path.Join(Config.Blueprints.Directory, string.Format("{0}.json", Name)));
    }


    public string Name;

    public MyGameObject BaseGameObject;

    public List<BlueprintComponent> Parts = new List<BlueprintComponent>();
}
