using System.Collections.Generic;
using UnityEngine;

public enum OrderType
{
    Attack,
    Construct,
    Destroy,
    Follow,
    Guard,
    Idle,
    Move,
    None,
    Load,
    Patrol,
    Produce,
    Rally,
    Research,
    Repair,
    Stop,
    Transport,
    Unload,
    Wait,
}

public class Order
{
    public Order(OrderType type, int maxRetries = 0)
    {
        Type = type;
        MaxRetries = maxRetries;
    }

    public Order(OrderType type, MyGameObject target, int maxRetries = 0)
    {
        Type = type;
        TargetGameObject = target;
        MaxRetries = maxRetries;
    }

    public Order(OrderType type, Vector3 target, int maxRetries = 0)
    {
        Type = type;
        TargetPosition = target;
        MaxRetries = maxRetries;
    }

    public Order(OrderType type, MyGameObject target, Dictionary<string, int> resources, float time, int maxRetries = 0)
    {
        Type = type;
        TargetGameObject = target;
        Resources = resources;
        Timer = new Timer(time);
        MaxRetries = maxRetries;
    }

    public Order(OrderType type, MyGameObject source, MyGameObject target, Dictionary<string, int> resources, int maxRetries = 0)
    {
        Type = type;
        SourceGameObject = source;
        TargetGameObject = target;
        Resources = resources;
        MaxRetries = maxRetries;
    }

    public Order(OrderType type, float time, int maxRetries = 0)
    {
        Type = type;
        Timer = new Timer(time);
        MaxRetries = maxRetries;
    }

    public Order(OrderType type, MyGameObject target, PrefabConstructionType prefabConstructionType, float time, int maxRetries = 0)
    {
        Type = type;
        TargetGameObject = target;
        PrefabConstructionType = prefabConstructionType;
        Timer = new Timer(time);
        MaxRetries = maxRetries;
    }

    public Order(OrderType type, string prefab, float time, int maxRetries = 0)
    {
        Type = type;
        Prefab = prefab;
        Timer = new Timer(time);
        MaxRetries = maxRetries;
    }

    public string GetInfo()
    {
        var info = string.Format("{0}", Type.ToString());

        if (Timer != null)
        {
            info += string.Format(" {0:0.}/{1}", Timer.Value, Timer.Max);
        }

        if (MaxRetries > 0)
        {
            info += string.Format(" {0}/{1}", Retries, MaxRetries);
        }

        return info;
    }

    public void Retry()
    {
        Retries += 1;
    }

    public OrderType Type { get; }

    public MyGameObject SourceGameObject { get; }

    public MyGameObject TargetGameObject { get; }

    public Vector3 TargetPosition { get; }

    public Dictionary<string, int> Resources { get; }

    public Timer Timer { get; }

    public string Prefab { get; }

    public PrefabConstructionType PrefabConstructionType { get; }

    public int Retries { get; private set; } = 1;

    public int MaxRetries { get; } = 0;

    public bool CanRetry { get => Retries < MaxRetries; }
}
