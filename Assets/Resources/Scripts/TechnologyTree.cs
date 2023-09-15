using System.Collections.Generic;
using UnityEngine;

public class TechnologyTree
{
    public void Load()
    {
        foreach (MyGameObject myGameObject in Resources.LoadAll<MyGameObject>(Config.DirectoryStructures))
        {
            Technologies[myGameObject.name] = new Technology(myGameObject.name);
            Technologies[myGameObject.name].Discovered = true;
        }

        foreach (MyGameObject myGameObject in Resources.LoadAll<MyGameObject>(Config.DirectoryUnits))
        {
            Technologies[myGameObject.name] = new Technology(myGameObject.name);
            Technologies[myGameObject.name].Discovered = true;
        }

        Technologies["Colonization"] = new Technology("Colonization", new HashSet<string> { "Electricity", "Factory_Light", "Harvester", "Headquarters", "Heavy_Industry", "Gas_Station", "Infantry", "Laser",  "Quad", "Radar 1", "Refinery", "Research_Lab", "Space_Travels", "Static_Defences", "Stationary_Defences", "Trike" });
        Technologies["Colonization"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Electricity"] = new Technology("Electricity", new HashSet<string> { "Power_Pole", "Windtrap" });
        Technologies["Electricity"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Infantry"] = new Technology("Infantry", new HashSet<string> { "Barracks", "Infantry_Light" });
        Technologies["Infantry"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Heavy_Industry"] = new Technology("Heavy_Industry", new HashSet<string> { "Factory_Heavy", "Tank_Missile" });
        Technologies["Heavy_Industry"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Laser"] = new Technology("Laser", new HashSet<string> { "Grav_Light", "Tank_Combat" });
        Technologies["Laser"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Radar 1"] = new Technology("Radar 1", new HashSet<string> { "Radar_Outpost", "Radar 2" });
        Technologies["Radar 1"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Radar 2"] = new Technology("Radar 2");
        Technologies["Radar 2"].Cost.Init("Crystal", 0, 40, ResourceDirection.In);

        Technologies["Space_Travels"] = new Technology("Space_Travels", new HashSet<string> { "Spaceport" });
        Technologies["Space_Travels"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Static_Defences"] = new Technology("Static_Defences", new HashSet<string> { "Wall" });
        Technologies["Static_Defences"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Stationary_Defences"] = new Technology("Stationary_Defences", new HashSet<string> { "Turret_Gun", "Turret_Missile" });
        Technologies["Stationary_Defences"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Discover("Colonization");
        Discover("Electricity");
        Discover("Infantry");
    }

    public bool IsDiscovered(string name)
    {
        return Technologies.ContainsKey(name) && Technologies[name].Discovered;
    }

    public bool IsUnlocked(string name)
    {
        return Technologies.ContainsKey(name) && Technologies[name].Unlocked;
    }

    public void Discover(string name)
    {
        if (Technologies.ContainsKey(name))
        {
            Technologies[name].Discovered = true;
            Technologies[name].Unlocked = true;

            foreach (string technology in Technologies[name].Unlocks)
            {
                if (Technologies.ContainsKey(technology))
                {
                    Technologies[technology].Unlocked = true;
                }
            }
        }
    }

    public Dictionary<string, Technology> Technologies = new Dictionary<string, Technology>();
}
