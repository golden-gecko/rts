using System.Collections.Generic;
using UnityEngine;

public class TechnologyTree
{
    public void Load()
    {
        // Load structures.
        MyGameObject[] structures = Resources.LoadAll<MyGameObject>("Objects/Structures");

        foreach (MyGameObject myGameObject in structures)
        {
            Technologies[myGameObject.name] = new Technology(myGameObject.name);
        }

        // Load units.
        MyGameObject[] units = Resources.LoadAll<MyGameObject>("Objects/Units");

        foreach (MyGameObject myGameObject in units)
        {
            Technologies[myGameObject.name] = new Technology(myGameObject.name);
        }

        // Create technologies to research.
        Dictionary<string, int> cost = new Dictionary<string, int>
        {
            { "Crystal", 50 },
        };

        Technologies["Colonization"] = new Technology("Colonization", cost, new HashSet<string> { "Factory_Light", "Grav_Light", "Harvester", "Headquarters", "Quad", "Refinery", "Research_Lab", "Trike" });
        Technologies["Infantry"] = new Technology("Infantry", cost, new HashSet<string> { "Barracks", "Infantry_Light" });
        Technologies["Heavy_Industry"] = new Technology("Heavy_Industry", cost, new HashSet<string> { "Factory_Heavy", "Tank_Combat", "Tank_Missile" });
        Technologies["Radar"] = new Technology("Radar", cost, new HashSet<string> { "Radar_Outpost" });
        Technologies["Space_Travels"] = new Technology("Space_Travels", cost, new HashSet<string> { "Spaceport" });
        Technologies["Static_Defences"] = new Technology("Static_Defences", cost, new HashSet<string> { "Wall" });
        Technologies["Stationary_Defences"] = new Technology("Stationary_Defences", cost, new HashSet<string> { "Turret_Gun", "Turret_Missile" });

        // Unlock starting technologies.
        Unlock("Colonization");
        Unlock("Infantry");
    }

    public Dictionary<string, int> GetCost(string name)
    {
        if (Technologies.ContainsKey(name))
        {
            return Technologies[name].Cost;
        }

        return new Dictionary<string, int>();
    }

    public bool IsUnlocked(string name)
    {
        return Technologies.ContainsKey(name) && Technologies[name].Unlocked;
    }

    public void Unlock(string name)
    {
        if (Technologies.ContainsKey(name))
        {
            Technologies[name].Unlocked = true;
        }

        UpdateTechnologies();
    }

    private void UpdateTechnologies()
    {
        List<string> queue = new List<string>(Technologies.Keys);

        while (queue.Count > 0)
        {
            if (Technologies.ContainsKey(queue[0]))
            {
                if (Technologies[queue[0]].Unlocked)
                {
                    foreach (string technology in Technologies[queue[0]].Unlocks)
                    {
                        if (Technologies.ContainsKey(technology))
                        {
                            Technologies[technology].Unlocked = true;

                            queue.Add(technology);
                        }
                    }
                }
            }

            queue.RemoveAt(0);
        }
    }

    public Dictionary<string, Technology> Technologies = new Dictionary<string, Technology>();
}
