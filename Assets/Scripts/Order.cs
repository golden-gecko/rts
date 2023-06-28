using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Order
{
    public static Order Attack(Vector3 position)
    {
        return new Order
        {
            Type = OrderType.Attack,
            TargetPosition = position,
        };
    }

    public static Order Attack(MyGameObject myGameObject)
    {
        return new Order
        {
            Type = OrderType.Attack,
            TargetGameObject = myGameObject,
        };
    }

    // Construct,

    public static Order Destroy()
    {
        return new Order
        {
            Type = OrderType.Destroy,
        };
    }

    public static Order Follow(MyGameObject myGameObject)
    {
        return new Order
        {
            Type = OrderType.Follow,
            TargetGameObject = myGameObject,
        };
    }

    public static Order Guard(Vector3 position)
    {
        return new Order
        {
            Type = OrderType.Guard,
            TargetPosition = position,
        };
    }

    public static Order Guard(MyGameObject myGameObject)
    {
        return new Order
        {
            Type = OrderType.Guard,
            TargetGameObject = myGameObject,
        };
    }

    // Load,

    public static Order Move(Vector3 position)
    {
        return new Order
        {
            Type = OrderType.Move,
            TargetPosition = position,
        };
    }

    public static Order Patrol(Vector3 position)
    {
        return new Order
        {
            Type = OrderType.Patrol,
            TargetPosition = position,
        };
    }

    public static Order Produce() // TODO: Add recipe name.
    {
        return new Order
        {
            Type = OrderType.Produce,
        };
    }

    public static Order Rally(Vector3 position)
    {
        return new Order
        {
            Type = OrderType.Rally,
            TargetPosition = position,
        };
    }

    public static Order Research() // TODO: Add research name.
    {
        return new Order
        {
            Type = OrderType.Research,
        };
    }

    public static Order Repair(MyGameObject myGameObject)
    {
        return new Order
        {
            Type = OrderType.Repair,
            TargetGameObject = myGameObject,
        };
    }

    public static Order Stop()
    {
        return new Order
        {
            Type = OrderType.Stop,
        };
    }

    // Transport,
    // Unload,

    public static Order Wait(float time)
    {
        return new Order
        {
            Type = OrderType.Wait,
            Timer = new Timer(time),
        };
    }

    private Order()
    {
    }

    /*
    public Order(OrderType type, int maxRetries = 0)
    {
        Type = type;
        MaxRetries = maxRetries;
    }

    public Order(OrderType type, MyGameObject target, int maxRetries = 0)
    {
        Type = type;
        IsTargetGameObject = true;
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
        IsTargetGameObject = true;
        TargetGameObject = target;
        Resources = resources;
        Timer = new Timer(time);
        MaxRetries = maxRetries;
    }

    public Order(OrderType type, MyGameObject source, MyGameObject target, Dictionary<string, int> resources, int maxRetries = 0)
    {
        Type = type;
        SourceGameObject = source;
        IsTargetGameObject = true;
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

    public Order(OrderType type, string prefab, PrefabConstructionType prefabConstructionType, float time, int maxRetries = 0, MyGameObject target = null)
    {
        Type = type;
        Prefab = prefab;
        PrefabConstructionType = prefabConstructionType;
        Timer = new Timer(time);
        MaxRetries = maxRetries;
        TargetGameObject = target;
    }

    public Order(OrderType type, string prefab, PrefabConstructionType prefabConstructionType, float time, int maxRetries, MyGameObject target, Vector3 targetPosition)
    {
        Type = type;
        Prefab = prefab;
        PrefabConstructionType = prefabConstructionType;
        Timer = new Timer(time);
        MaxRetries = maxRetries;
        TargetGameObject = target;
        TargetPosition = targetPosition;
    }

    public Order(OrderType type, string prefab, PrefabConstructionType prefabConstructionType, Vector3 target, float time, int maxRetries = 0)
    {
        Type = type;
        Prefab = prefab;
        PrefabConstructionType = prefabConstructionType;
        TargetPosition = target;
        Timer = new Timer(time);
        MaxRetries = maxRetries;
    }
    */

    public string GetInfo()
    {
        string info = string.Format("{0}", Type.ToString());

        if (Timer != null)
        {
            info += string.Format(" {0:0.}/{1}", Timer.Current, Timer.Max);
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

    public bool CanRetry { get => Retries < MaxRetries; }

    public bool IsTargetGameObject { get; private set; }

    public int MaxRetries { get; private set; } = 0;

    public string Prefab { get; private set; }

    public PrefabConstructionType PrefabConstructionType { get; private set; }

    public Dictionary<string, int> Resources { get; private set; }

    public int Retries { get; private set; } = 0;

    public MyGameObject SourceGameObject { get; private set; }

    public MyGameObject TargetGameObject { get; private set; }

    public Vector3 TargetPosition { get; private set; }

    public Timer Timer { get; private set; }

    public OrderType Type { get; private set; }
}
