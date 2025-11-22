using System.Collections.Generic;

public class Stats
{
    public static string DamageDealt = "Damage Dealt";
    public static string DamageRepaired = "Damage Repaired";
    public static string DamageTaken = "Damage Taken";
    public static string DistanceDriven = "Distance Driven";
    public static string MissilesFired = "Missiles Fired";
    public static string OrdersCancelled = "Orders Cancelled"; // TODO: Increase when order queue is cleared.
    public static string OrdersExecuted = "Orders Executed";
    public static string OrdersFailed = "Orders Failed";
    public static string ResourcesProduced = "Resources Produced";
    public static string ResourcesTransported = "Resources Transported";
    public static string ResourcesUsed = "Resources Used";
    public static string SkillsUsed = "Skills Used";
    public static string TargetsDestroyed = "Targets Destroyed";
    public static string TechnologiesDiscovered = "Technologies Discovered";
    public static string TimeConstructing = "Time Constructing";
    public static string TimeProducing = "Time Producing";
    public static string TimeResearching = "Time Researching";
    public static string TimeWaiting = "Time Waiting";

    public void Add(string name, float value) // TODO: Some values are passed as integers.
    {
        if (Items.ContainsKey(name))
        {
            Items[name] += value;
        }
        else
        {
            Items[name] = value;
        }
    }

    public string GetInfo()
    {
        string info = string.Empty;

        foreach (KeyValuePair<string, float> i in Items)
        {
            info += string.Format("\n  {0}: {1:0.}", i.Key, i.Value);
        }

        return info;
    }

    public Dictionary<string, float> Items { get; } = new Dictionary<string, float>();
}
