using System.Collections.Generic;
using UnityEngine;

public enum OrderType
{
    Attack,
    Construct,
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
    Stop,
    Transport,
    Unload,
}

public class Order
{
    public Order(OrderType type)
    {
        Type = type;
    }

    public Order(OrderType type, MyGameObject target)
    {
        Type = type;
        TargetGameObject = target;
    }

    public Order(OrderType type, Vector3 target)
    {
        Type = type;
        TargetPosition = target;
    }

    public Order(OrderType type, MyGameObject target, Dictionary<string, int> resources)
    {
        Type = type;
        TargetGameObject = target;
        Resources = resources;
    }

    public Order(OrderType type, MyGameObject source, MyGameObject target, Dictionary<string, int> resources)
    {
        Type = type;
        SourceGameObject = source;
        TargetGameObject = target;
        Resources = resources;
    }

    public Order(OrderType type, float max)
    {
        Type = type;
        Timer = new Timer(max);
    }

    public string GetInfo()
    {
        if (Timer == null)
        {
            return string.Format("{0}", Type.ToString());
        }

        return string.Format("{0} ({1:0.}/{2})", Type.ToString(), Timer.Value, Timer.Max);
    }

    public OrderType Type { get; }

    public MyGameObject SourceGameObject { get; }

    public MyGameObject TargetGameObject { get; }

    public Vector3 TargetPosition { get; }

    public Dictionary<string, int> Resources { get; private set; }

    public Timer Timer { get; }
}
