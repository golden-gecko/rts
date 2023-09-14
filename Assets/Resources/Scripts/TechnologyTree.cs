using System.Collections.Generic;
using UnityEngine;

public class TechnologyTree
{
    public void Load()
    {
        // Load structures.
        MyGameObject[] structures = Resources.LoadAll<MyGameObject>(Config.DirectoryStructures);

        foreach (MyGameObject myGameObject in structures)
        {
            Technologies[myGameObject.name] = new Technology(myGameObject.name, "");
        }

        // Load units.
        MyGameObject[] units = Resources.LoadAll<MyGameObject>(Config.DirectoryUnits);

        foreach (MyGameObject myGameObject in units)
        {
            Technologies[myGameObject.name] = new Technology(myGameObject.name, "");
        }

        // Create technologies to research.
        Technologies["Colonization"] = new Technology("Colonization", "", new HashSet<string> { "Factory_Light", "Harvester", "Headquarters", "Gas_Station", "Quad", "Refinery", "Research_Lab", "Trike" });
        Technologies["Colonization"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Electricity"] = new Technology("Electricity", "", new HashSet<string> { "Power_Pole", "Windtrap" });
        Technologies["Electricity"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Infantry"] = new Technology("Infantry", "", new HashSet<string> { "Barracks", "Infantry_Light" });
        Technologies["Infantry"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Heavy_Industry"] = new Technology("Heavy_Industry", "", new HashSet<string> { "Factory_Heavy", "Tank_Missile" });
        Technologies["Heavy_Industry"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Laser"] = new Technology("Laser", "", new HashSet<string> { "Grav_Light", "Tank_Combat" });
        Technologies["Laser"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Radar 1"] = new Technology("Radar 1", "", new HashSet<string> { "Radar_Outpost" });
        Technologies["Radar 1"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Radar 2"] = new Technology("Radar 2", "Radar is able to detect object size."); // TODO: This tech should be locked until "Radar 1" is discovered.
        Technologies["Radar 2"].Cost.Init("Crystal", 0, 40, ResourceDirection.In);

        Technologies["Space_Travels"] = new Technology("Space_Travels", "", new HashSet<string> { "Spaceport" });
        Technologies["Space_Travels"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Static_Defences"] = new Technology("Static_Defences", "", new HashSet<string> { "Wall" });
        Technologies["Static_Defences"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Stationary_Defences"] = new Technology("Stationary_Defences", "", new HashSet<string> { "Turret_Gun", "Turret_Missile" });
        Technologies["Stationary_Defences"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        // Unlock starting technologies.
        Unlock("Colonization");
        Unlock("Electricity");
        Unlock("Infantry");
    }

    public ResourceContainer GetCost(string name)
    {
        return Technologies[name].Cost;
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
