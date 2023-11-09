using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public static Order Assemble(string prefab)
    {
        return new Order
        {
            Type = OrderType.Assemble,
            Prefab = prefab,
        };
    }

    public static Order AttackObject(MyGameObject myGameObject)
    {
        return new Order
        {
            Type = OrderType.AttackObject,
            TargetGameObject = myGameObject,
        };
    }

    public static Order AttackPosition(Vector3 position)
    {
        return new Order
        {
            Type = OrderType.AttackPosition,
            TargetPosition = position,
        };
    }

    public static Order Construct(MyGameObject myGameObject)
    {
        return new Order
        {
            Type = OrderType.Construct,
            TargetGameObject = myGameObject,
        };
    }

    public static Order Destroy()
    {
        return new Order
        {
            Type = OrderType.Destroy,
        };
    }

    public static Order Disable()
    {
        return new Order
        {
            Type = OrderType.Disable,
        };
    }

    public static Order Enable()
    {
        return new Order
        {
            Type = OrderType.Enable,
        };
    }

    public static Order Explore(Vector3 position)
    {

        return new Order
        {
            Type = OrderType.Explore,
            TargetPosition = position,
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

    public static Order GatherObject(MyGameObject myGameObject)
    {
        return new Order
        {
            Type = OrderType.GatherObject,
            TargetGameObject = myGameObject,
        };
    }

    public static Order GatherResource(string resource)
    {
        return new Order
        {
            Type = OrderType.GatherResource,
            Resource = resource,
        };
    }

    public static Order GuardObject(MyGameObject myGameObject)
    {
        return new Order
        {
            Type = OrderType.GuardObject,
            TargetGameObject = myGameObject,
        };
    }

    public static Order GuardPosition(Vector3 position)
    {
        return new Order
        {
            Type = OrderType.GuardPosition,
            TargetPosition = position,
        };
    }

    public static Order Load(MyGameObject myGameObject, string resource, int value)
    {
        return new Order
        {
            Type = OrderType.Load,
            SourceGameObject = myGameObject,
            Resource = resource,
            Value = value,
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

    public static Order Produce(string recipe)
    {
        return new Order
        {
            Type = OrderType.Produce,
            Recipe = recipe,
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

    public static Order Research(string technology)
    {
        return new Order
        {
            Type = OrderType.Research,
            Technology = technology,
        };
    }

    public static Order Stock(MyGameObject myGameObject, string resource, int value)
    {
        return new Order
        {
            Type = OrderType.Stock,
            TargetGameObject = myGameObject,
            Resource = resource,
            Value = value,
        };
    }

    public static Order Stop()
    {
        return new Order
        {
            Type = OrderType.Stop,
        };
    }

    public static Order Teleport(MyGameObject sourceGameObject, MyGameObject targetGameObject)
    {
        return new Order
        {
            Type = OrderType.Teleport,
            SourceGameObject = sourceGameObject,
            TargetGameObject = targetGameObject,
            State = OrderState.GoToEntrance,
        };
    }

    public static Order Transport(MyGameObject sourceGameObject, MyGameObject targetGameObject, string resource, int value)
    {
        return new Order
        {
            Type = OrderType.Transport,
            SourceGameObject = sourceGameObject,
            TargetGameObject = targetGameObject,
            Resource = resource,
            Value = value,
        };
    }

    public static Order Turn(Vector3 position)
    {
        return new Order
        {
            Type = OrderType.Turn,
            TargetPosition = position,
        };
    }

    public static Order Unload(MyGameObject myGameObject, string resource, int value)
    {
        return new Order
        {
            Type = OrderType.Unload,
            TargetGameObject = myGameObject,
            Resource = resource,
            Value = value,
        };
    }

    public static Order UseSkill(string skill)
    {
        return new Order
        {
            Type = OrderType.UseSkill,
            Skill = skill,
        };
    }

    public static Order Wait()
    {
        return new Order
        {
            Type = OrderType.Wait,
        };
    }

    private Order()
    {
    }

    public string GetInfo()
    {
        string info = string.Format("{0}", Type.ToString());

        if (Prefab != null && Prefab.Length > 0)
        {
            info += string.Format(" ({0})", Prefab);
        }

        if (Recipe != null && Recipe.Length > 0)
        {
            info += string.Format(" ({0})", Recipe);
        }

        if (Resource != null && Resource.Length > 0)
        {
            info += string.Format(" ({0})", Resource);
        }

        if (Skill != null && Skill.Length > 0)
        {
            info += string.Format(" ({0})", Skill);
        }

        if (Technology != null && Technology.Length > 0)
        {
            info += string.Format(" ({0})", Technology);
        }

        if (Timer != null)
        {
            info += string.Format(" {0}", Timer.GetInfo());
        }

        return info;
    }

    public void Cancel()
    {
        switch (Type)
        {
            case OrderType.Assemble:
                if (TargetGameObject != null)
                {
                    TargetGameObject.OnDestroyHandler();

                    // TODO: Which one is correct?
                    // TargetGameObject.Destroy(0);
                }
                break;
        }
    }

    public bool IsValid()
    {
        switch (Type)
        {
            case OrderType.Assemble:
                return Prefab != null && Prefab.Length > 0;

            case OrderType.AttackObject:
            case OrderType.Follow:
            case OrderType.GatherObject:
            case OrderType.GuardObject:
            case OrderType.Stock:
            case OrderType.Unload:
                return TargetGameObject != null;

            case OrderType.GatherResource:
                return Resource != null && Resource.Length > 0;

            case OrderType.Load:
                return SourceGameObject != null;

            case OrderType.Produce:
                return Recipe != null && Recipe.Length > 0;

            case OrderType.Research:
                return Technology != null && Technology.Length > 0;

            case OrderType.Transport:
                return SourceGameObject != null && TargetGameObject != null;

            case OrderType.UseSkill:
                return Skill != null && Skill.Length > 0;
        }

        return true;
    }

    public MyGameObject SourceGameObject { get; set; }

    public MyGameObject TargetGameObject { get; set; }

    public Vector3 TargetPosition { get; private set; }

    public Quaternion TargetRotation { get; private set; }

    public Timer Timer { get; set; }

    public OrderType Type { get; private set; }

    public string Prefab { get; private set; }

    public string Recipe { get; set; }

    public string Resource { get; private set; }

    public int Value { get; private set; }

    public string Skill { get; private set; }

    public string Technology { get; private set; }

    public List<Vector3Int> Queue { get; set; }

    public List<Vector3Int> Visited { get; set; }

    public OrderState State { get; set; } = OrderState.None;
}
