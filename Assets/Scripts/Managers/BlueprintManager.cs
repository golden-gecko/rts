using System.Collections.Generic;
using UnityEngine;

public class BlueprintManager : MonoBehaviour
{
    private void Awake()
    {
        Blueprints = Utils.CreateBlueprints();
    }

    public Blueprint Get(string name)
    {
        return Blueprints.Find(x => x.Name == name);
    }

    public void Save(Blueprint blueprint)
    {
        Delete(blueprint.Name);
        Blueprints.Add(blueprint);
    }

    public void Delete(string name)
    {
        Blueprint blueprint = Get(name);

        if (blueprint != null)
        {
            blueprint.Delete();

            Blueprints.Remove(blueprint);
        }
    }

    public List<Blueprint> Blueprints = new List<Blueprint>();
}
