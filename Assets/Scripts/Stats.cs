using System.Collections.Generic;

public class Stats
{
    public static string DamageDealt = "Damage Dealt";
    public static string DamageTaken = "Damage Taken";
    public static string DistanceDriven = "Distance Driven";
    public static string MissilesFired = "Missiles Fired";
    public static string OrdersExecuted = "Orders Executed";
    public static string OrdersFailed = "Orders Failed";
    public static string ResourcesProduced = "Resources Produced";
    public static string ResourcesTransported = "Resources Transported";
    public static string TargetsDestroyed = "Targets Destroyed";
    public static string TimeConstructing = "Time Constructing";
    public static string TimeProducing = "Time Producing";
    public static string TimeWaiting = "Time Waiting";

    public Stats()
    {
        Items = new Dictionary<string, float>();
    }

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

    public Dictionary<string, float> Items { get; }
}
