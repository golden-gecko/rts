using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;

public class TechnologyTree
{
    public TechnologyTree()
    {
        Dictionary<string, int> cost = new Dictionary<string, int>
        {
            { "Crystal", 50 },
        };

        // Structures
        Technologies["Barracks"] = new Technology("Barracks", false);
        Technologies["Factory_Heavy"] = new Technology("Factory_Heavy", false);
        Technologies["Factory_Light"] = new Technology("Factory_Light", false);
        Technologies["Headquarters"] = new Technology("Headquarters", false);
        Technologies["Misc_Building"] = new Technology("Misc_Building", false);
        Technologies["Misc_Building"] = new Technology("Misc_Building", false);
        Technologies["Radar_Outpost"] = new Technology("Radar_Outpost", false);
        Technologies["Refinery"] = new Technology("Refinery", false);
        Technologies["Research_Lab"] = new Technology("Research_Lab", false);
        Technologies["Spaceport"] = new Technology("Spaceport", false);
        Technologies["Turret_Gun"] = new Technology("Turret_Gun", false);
        Technologies["Turret_Missile"] = new Technology("Turret_Missile", false);
        Technologies["Wall"] = new Technology("Wall", false);

        // Units
        Technologies["Grav_Light"] = new Technology("Grav_Light", false);
        Technologies["Harvester"] = new Technology("Harvester", false);
        Technologies["Infantry_Light"] = new Technology("Infantry_Light", false);
        Technologies["Quad"] = new Technology("Quad", false);
        Technologies["Tank_Combat"] = new Technology("Tank_Combat", false);
        Technologies["Tank_Missile"] = new Technology("Tank_Missile", false);
        Technologies["Trike"] = new Technology("Trike", false);

        // Technologies
        Technologies["Colonization"] = new Technology("Colonization", cost, true, new List<string> {"Barracks", "Factory_Light", "Headquarters", "Misc_Building", "Misc_Building", "Refinery", "Research_Lab" });
        Technologies["Heavy Industry"] = new Technology("Heavy Industry", cost, false, new List<string> {
            "Factory_Heavy",
        });

        Technologies["Radar"] = new Technology("Radar", cost, false, new List<string> {
            "Radar_Outpost",
        });

        Technologies["Space Travels"] = new Technology("Space Travels", cost, false, new List<string> {
            "Spaceport",
        });

        Technologies["Static Defences"] = new Technology("Static Defences", cost, false, new List<string> {
            "Wall",
        });
        Technologies["Stationary Defences"] = new Technology("Stationary Defences", cost, false, new List<string> { "Turret_Gun", "Turret_Missile" });

        UpdateTechnologies();
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
        if (Technologies.ContainsKey(name) && Technologies[name].Unlocked == false)
        {
            Technologies[name].Unlocked = true;

            UpdateTechnologies();
        }
    }

    private void UpdateTechnologies()
    {
        foreach (KeyValuePair<string, Technology> i in Technologies)
        {
            if (i.Value.Unlocked)
            {
                foreach (string name in i.Value.Unlocks)
                {
                    Unlock(name);
                }
            }
        }
    }

    public Dictionary<string, Technology> Technologies = new Dictionary<string, Technology>();
}
