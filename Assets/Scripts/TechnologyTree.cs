using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TechnologyTree
{
    public void Load()
    {
        Load(Config.Instance.Structures);
        Load(Config.Instance.Units);

        // Starting technologies.
        Technologies["Colonization"] = new Technology("Colonization");
        Technologies["Colonization"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Electricity"] = new Technology("Electricity", new HashSet<string> { "Colonization" });
        Technologies["Electricity"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        // Flight.
        Technologies["Flight"] = new Technology("Flight", new HashSet<string> { "Electricity" });
        Technologies["Flight"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Caryall"] = new Technology("Caryall", new HashSet<string> { "Flight" });
        Technologies["Ornithopter_Light"] = new Technology("Ornithopter_Light", new HashSet<string> { "Flight" });

        // Heavy industry.
        Technologies["Heavy_Industry"] = new Technology("Heavy_Industry", new HashSet<string> { "Electricity" });
        Technologies["Heavy_Industry"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Factory_Heavy"] = new Technology("Factory_Heavy", new HashSet<string> { "Heavy_Industry" });
        Technologies["Tank_Combat"] = new Technology("Tank_Combat", new HashSet<string> { "Heavy_Industry", "Laser" });
        Technologies["Tank_Missile"] = new Technology("Tank_Missile", new HashSet<string> { "Heavy_Industry" });
        Technologies["Tank_Special"] = new Technology("Tank_Special", new HashSet<string> { "Heavy_Industry" });

        // Infantry.
        Technologies["Infantry"] = new Technology("Infantry", new HashSet<string> { "Electricity" });
        Technologies["Infantry"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Barracks"] = new Technology("Barracks", new HashSet<string> { "Infantry" });
        Technologies["Infantry_Light"] = new Technology("Infantry_Light", new HashSet<string> { "Infantry" });

        // Laser.
        Technologies["Laser"] = new Technology("Laser", new HashSet<string> { "Electricity" });
        Technologies["Laser"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Grav_Light"] = new Technology("Laser", new HashSet<string> { "Laser" });
        Technologies["Turret_Gun"] = new Technology("Laser", new HashSet<string> { "Laser", "Stationary_Defences" });

        // Radar.
        Technologies["Radar_1"] = new Technology("Radar1", new HashSet<string> { "Electricity" });
        Technologies["Radar_1"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Radar_2"] = new Technology("Radar_2", new HashSet<string> { "Radar_1" });
        Technologies["Radar_2"].Cost.Init("Crystal", 0, 40, ResourceDirection.In);

        Technologies["Radar_3"] = new Technology("Radar_3", new HashSet<string> { "Radar_2" });
        Technologies["Radar_3"].Cost.Init("Crystal", 0, 60, ResourceDirection.In);

        Technologies["Radar_Outpost"] = new Technology("Radar_Outpost", new HashSet<string> { "Radar_1" });
        Technologies["Radar_Outpost"].Cost.Init("Crystal", 0, 60, ResourceDirection.In);

        // Space travels.
        Technologies["Space_Travels"] = new Technology("Space_Travels", new HashSet<string> { "Heavy_Industry" });
        Technologies["Space_Travels"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Spaceport"] = new Technology("Space_Travels", new HashSet<string> { "Space_Travels" });
        Technologies["Spaceport"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        // Static defences.
        Technologies["Static_Defences"] = new Technology("Static_Defences", new HashSet<string> { "Colonization" });
        Technologies["Static_Defences"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Wall"] = new Technology("Static_Defences", new HashSet<string> { "Static_Defences" });
        Technologies["Wall"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        // Stationary defences.
        Technologies["Stationary_Defences"] = new Technology("Stationary_Defences", new HashSet<string> { "Turret_Missile" });
        Technologies["Stationary_Defences"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Turret_Missile"] = new Technology("Turret_Missile", new HashSet<string> { "Stationary_Defences" });
        Technologies["Turret_Missile"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        // Submarines.
        Technologies["Submarines"] = new Technology("Submarines", new HashSet<string> { "Electricity" });
        Technologies["Submarines"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Submarine"] = new Technology("Laser", new HashSet<string> { "Submarines" });

        DiscoverStartingTechnologies();
    }

    public bool IsDiscovered(string name)
    {
        if (Technologies.TryGetValue(name, out Technology technology))
        {
            if (AreRequirementsDiscovered(technology) == false)
            {
                return false;
            }

            return technology.Discovered;
        }

        return false;
    }

    public bool IsReadyToDiscover(string name)
    {
        if (Technologies.TryGetValue(name, out Technology technology))
        {
            if (AreRequirementsDiscovered(technology) == false)
            {
                return false;
            }

            return technology.Discovered == false;
        }

        return false;
    }

    public void Discover(string name)
    {
        if (Technologies.TryGetValue(name, out Technology technology))
        {
            technology.Discovered = true;
        }
    }

    private bool AreRequirementsDiscovered(Technology technology)
    {
        foreach (string requirement in technology.Requirements)
        {
            if (Technologies.Values.Any(x => x.Name == requirement && x.Discovered) == false)
            {
                return false;
            }
        }

        return true;
    }

    private void DiscoverStartingTechnologies()
    {
        Discover("Colonization");
        Discover("Electricity");
        Discover("Infantry");
    }

    private void Load(List<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            Technologies[gameObject.name] = new Technology(gameObject.name) { Discovered = true };
        }
    }

    public Dictionary<string, Technology> Technologies { get; } = new Dictionary<string, Technology>();
}
