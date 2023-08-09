using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public static Order Assemble(string prefab, float time)
    {
        return new Order
        {
            Type = OrderType.Assemble,
            Prefab = prefab,
            Timer = new Timer(time),
        };
    }

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
            IsTargetGameObject = true, // TODO: Get rid of this. Maybe add a new order type.
        };
    }

    public static Order Construct(MyGameObject myGameObject, float time)
    {
        return new Order
        {
            Type = OrderType.Construct,
            TargetGameObject = myGameObject,
            Timer = new Timer(time),
        };
    }

    public static Order Construct(string prefab, MyGameObject myGameObject, float time)
    {
        return new Order
        {
            Type = OrderType.Construct,
            Prefab = prefab,
            TargetGameObject = myGameObject,
            Timer = new Timer(time),
        };
    }

    public static Order Destroy()
    {
        return new Order
        {
            Type = OrderType.Destroy,
        };
    }

    public static Order Explore()
    {

        return new Order
        {
            Type = OrderType.Explore,
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

    public static Order Load(MyGameObject myGameObject, Dictionary<string, int> resources, float time)
    {
        return new Order
        {
            Type = OrderType.Load,
            SourceGameObject = myGameObject,
            Resources = resources,
            Timer = new Timer(time),
        };
    }

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

    public static Order Produce(string recipe, float time)
    {
        return new Order
        {
            Type = OrderType.Produce,
            Recipe = recipe,
            Timer = new Timer(time),
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

    public static Order Research(string technology, float time)
    {
        return new Order
        {
            Type = OrderType.Research,
            Technology = technology,
            Timer = new Timer(time),
        };
    }

    public static Order Skill(string skill)
    {
        return new Order
        {
            Type = OrderType.Skill,
            Prefab = skill, // TODO: Add new property for skill.
        };
    }

    public static Order Stop()
    {
        return new Order
        {
            Type = OrderType.Stop,
        };
    }

    public static Order Transport(MyGameObject sourceGameObject, MyGameObject targetGameObject, Dictionary<string, int> resources, float time)
    {
        return new Order
        {
            Type = OrderType.Transport,
            SourceGameObject = sourceGameObject,
            TargetGameObject = targetGameObject,
            Resources = resources,
            Timer = new Timer(time),
        };
    }

    public static Order Unload(MyGameObject myGameObject, Dictionary<string, int> resources, float time)
    {
        return new Order
        {
            Type = OrderType.Unload,
            TargetGameObject = myGameObject,
            Resources = resources,
            Timer = new Timer(time),
        };
    }

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

    public string GetInfo()
    {
        string info = string.Format("{0}", Type.ToString());

        if (Prefab != null)
        {
            info += string.Format(" ({0})", Prefab);
        }

        if (Technology != null)
        {
            info += string.Format(" ({0})", Technology);
        }

        if (Recipe != null)
        {
            info += string.Format(" ({0})", Recipe);
        }

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

    public MyGameObject TargetGameObject { get; set; } // TODO: Hide setter.

    public Vector3 TargetPosition { get; private set; }

    public Timer Timer { get; private set; }

    public OrderType Type { get; private set; }

    public string Technology { get; private set; }

    public string Recipe { get; private set; }
}
