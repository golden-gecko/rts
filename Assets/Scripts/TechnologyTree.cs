using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TechnologyTree
{
    public void Load()
    {
        Load(Game.Instance.Config.Structures);
        Load(Game.Instance.Config.Units);

        // Colonization.
        Technologies["Colonization"] = new Technology("Colonization");
        Technologies["Colonization"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        // Electricity.
        Technologies["Electricity"] = new Technology("Electricity", new HashSet<string> { "Colonization" });
        Technologies["Electricity"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Power_Pole"].Requirements.Add("Electricity");

        // Flight.
        Technologies["Flight"] = new Technology("Flight", new HashSet<string> { "Electricity" });
        Technologies["Flight"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Caryall"].Requirements.Add("Flight");
        Technologies["Ornithopter_Light"].Requirements.Add("Flight");

        // Foundations
        Technologies["Foundations"] = new Technology("Foundations");
        Technologies["Foundations"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Foundation_1x1"].Requirements.Add("Foundations");
        //Technologies["Foundation_2x2"].Requirements.Add("Foundations");
        //Technologies["Foundation_3x3"].Requirements.Add("Foundations");

        // Heavy industry.
        Technologies["Heavy_Industry"] = new Technology("Heavy_Industry", new HashSet<string> { "Electricity" });
        Technologies["Heavy_Industry"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Factory_Heavy"].Requirements.Add("Heavy_Industry");
        Technologies["Tank_Combat"].Requirements.Add("Heavy_Industry");
        Technologies["Tank_Combat"].Requirements.Add("Laser");
        Technologies["Tank_Missile"].Requirements.Add("Heavy_Industry");
        Technologies["Tank_Special"].Requirements.Add("Heavy_Industry");

        // Infantry.
        Technologies["Infantry"] = new Technology("Infantry", new HashSet<string> { "Electricity" });
        Technologies["Infantry"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Barracks"].Requirements.Add("Infantry");
        Technologies["Infantry_Light"].Requirements.Add("Infantry");

        // Laser.
        Technologies["Laser"] = new Technology("Laser", new HashSet<string> { "Electricity" });
        Technologies["Laser"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Grav_Light"].Requirements.Add("Laser");
        Technologies["Turret_Gun"].Requirements.Add("Laser");
        Technologies["Turret_Gun"].Requirements.Add("Stationary_Defences");

        // Radar.
        Technologies["Radar_1"] = new Technology("Radar1", new HashSet<string> { "Electricity" });
        Technologies["Radar_1"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Radar_2"] = new Technology("Radar_2", new HashSet<string> { "Radar_1" });
        Technologies["Radar_2"].Cost.Init("Crystal", 0, 40, ResourceDirection.In);

        Technologies["Radar_3"] = new Technology("Radar_3", new HashSet<string> { "Radar_2" });
        Technologies["Radar_3"].Cost.Init("Crystal", 0, 60, ResourceDirection.In);

        Technologies["Radar_Outpost"].Requirements.Add("Radar_1");

        // Space travels.
        Technologies["Space_Travels"] = new Technology("Space_Travels", new HashSet<string> { "Heavy_Industry" });
        Technologies["Space_Travels"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Spaceport"].Requirements.Add("Space_Travels");

        // Static defences.
        Technologies["Static_Defences"] = new Technology("Static_Defences", new HashSet<string> { "Colonization" });
        Technologies["Static_Defences"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Wall"].Requirements.Add("Static_Defences");

        // Stationary defences.
        Technologies["Stationary_Defences"] = new Technology("Stationary_Defences", new HashSet<string> { "Turret_Missile" });
        Technologies["Stationary_Defences"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        // Submarines.
        Technologies["Submarines"] = new Technology("Submarines", new HashSet<string> { "Electricity" });
        Technologies["Submarines"].Cost.Init("Crystal", 0, 20, ResourceDirection.In);

        Technologies["Submarine"].Requirements.Add("Submarines");

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
        Discover("Foundations");
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
