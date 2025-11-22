using System.Collections.Generic;

public class TechnologyTree
{
    public TechnologyTree()
    {
        // Structures
        Technologies["struct_Barracks_A_yup"] = new Technology("struct_Barracks_A_yup", false, false);
        Technologies["struct_Factory_Heavy_A_yup"] = new Technology("struct_Factory_Heavy_A_yup", false, false);
        Technologies["struct_Factory_Light_A_yup"] = new Technology("struct_Factory_Light_A_yup", false, false);
        Technologies["struct_Headquarters_A_yup"] = new Technology("struct_Headquarters_A_yup", false, false);
        Technologies["struct_Misc_Building_A_yup"] = new Technology("struct_Misc_Building_A_yup", false, false);
        Technologies["struct_Misc_Building_B_yup"] = new Technology("struct_Misc_Building_B_yup", false, false);
        Technologies["struct_Radar_Outpost_A_yup"] = new Technology("struct_Radar_Outpost_A_yup", false, false);
        Technologies["struct_Refinery_A_yup"] = new Technology("struct_Refinery_A_yup", false, false);
        Technologies["struct_Research_Lab_A_yup"] = new Technology("struct_Research_Lab_A_yup", false, false);
        Technologies["struct_Spaceport_A_yup"] = new Technology("struct_Spaceport_A_yup", false, false);
        Technologies["struct_Turret_Gun_A_yup"] = new Technology("struct_Turret_Gun_A_yup", false, false);
        Technologies["struct_Turret_Missile_A_yup"] = new Technology("struct_Turret_Missile_A_yup", false, false);
        Technologies["struct_Wall_A_yup"] = new Technology("struct_Wall_A_yup", false, false);

        // Units
        Technologies["unit_Grav_Light_A_yup"] = new Technology("unit_Grav_Light_A_yup", false, false);
        Technologies["unit_Harvester_A_yup"] = new Technology("unit_Harvester_A_yup", false, false);
        Technologies["unit_Infantry_Light_B_yup"] = new Technology("unit_Infantry_Light_B_yup", false, false);
        Technologies["unit_Quad_A_yup"] = new Technology("unit_Quad_A_yup", false, false);
        Technologies["unit_Tank_Combat_A_yup"] = new Technology("unit_Tank_Combat_A_yup", false, false);
        Technologies["unit_Tank_Missile_A_yup"] = new Technology("unit_Tank_Missile_A_yup", false, false);
        Technologies["unit_Trike_A_yup"] = new Technology("unit_Trike_A_yup", false, false);

        // Technologies
        Technologies["Colonization"] = new Technology("Colonization", true, true, new List<string> {
            "struct_Barracks_A_yup",
            "struct_Factory_Light_A_yup",
            "struct_Headquarters_A_yup",
            "struct_Misc_Building_A_yup",
            "struct_Misc_Building_B_yup",
            "struct_Refinery_A_yup",
            "struct_Research_Lab_A_yup",
        });

        Technologies["Heavy Industry"] = new Technology("Heavy Industry", true, false, new List<string> {
            "struct_Factory_Heavy_A_yup",
        });

        Technologies["Radar"] = new Technology("Radar", true, false, new List<string> {
            "struct_Radar_Outpost_A_yup",
        });

        Technologies["Space Travels"] = new Technology("Space Travels", true, false, new List<string> {
            "struct_Spaceport_A_yup",
        });

        Technologies["Static Defences"] = new Technology("Static Defences", true, false, new List<string> {
            "struct_Wall_A_yup",
        });

        Technologies["Stationary Defences"] = new Technology("Stationary Defences", true, false, new List<string> {
            "struct_Turret_Gun_A_yup",
            "struct_Turret_Missile_A_yup",
        });

        UpdateTechnologies();
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
