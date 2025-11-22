using System.Collections.Generic;
using System.Linq;

public class Stats
{
    public static string DamageDealt = "Damage Dealt";
    public static string DamageRepaired = "Damage Repaired";
    public static string DamageTaken = "Damage Taken";
    public static string DistanceDriven = "Distance Driven";
    public static string MissilesFired = "Missiles Fired";
    public static string ObjectsAssembled = "Objects Assembled";
    public static string ObjectsConstructed = "Objects Constructed";
    public static string OrdersCancelled = "Orders Cancelled";
    public static string OrdersCompleted = "Orders Completed";
    public static string OrdersFailed = "Orders Failed";
    public static string ResourcesProduced = "Resources Produced";
    public static string ResourcesTransported = "Resources Transported";
    public static string ResourcesUsed = "Resources Used";
    public static string SkillsUsed = "Skills Used";
    public static string TargetsDestroyed = "Targets Destroyed";
    public static string TechnologiesDiscovered = "Technologies Discovered";
    public static string TimeAssembling = "Time Assembling";
    public static string TimeConstructing = "Time Constructing";
    public static string TimeProducing = "Time Producing";
    public static string TimeResearching = "Time Researching";
    public static string TimeWaiting = "Time Waiting";

    public void Add(string name, float value)
    {
        if (Items.ContainsKey(name))
        {
            Items[name] += value;
        }
        else
        {
            Items[name] = value;
        }

        if (Player != null)
        {
            Player.Stats.Add(name, value);
        }
    }

    public void Inc(string name)
    {
        Add(name, 1.0f);
    }

    public float Get(string name)
    {
        if (Items.ContainsKey(name) == false)
        {
            return 0.0f;
        }

        return Items[name];
    }

    public string GetInfo()
    {
        string info = string.Empty;

        foreach (KeyValuePair<string, float> i in Items.OrderBy(x => x.Key))
        {
            info += string.Format("\n  {0}: {1:0.}", i.Key, i.Value);
        }

        return info;
    }

    public Player Player { get; set; }

    public Dictionary<string, float> Items { get; } = new Dictionary<string, float>();
}
