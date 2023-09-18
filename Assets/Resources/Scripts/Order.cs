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
            IsTargetGameObject = true,
        };
    }

    public static Order Construct(MyGameObject myGameObject)
    {
        return new Order
        {
            Type = OrderType.Construct,
            TargetGameObject = myGameObject,
            IsTargetGameObject = true,
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
            IsTargetGameObject = true,
        };
    }

    public static Order Gather(MyGameObject myGameObject)
    {
        return new Order
        {
            Type = OrderType.Gather,
            TargetGameObject = myGameObject,
            IsTargetGameObject = true,
        };
    }

    public static Order Gather(string resource)
    {
        return new Order
        {
            Type = OrderType.Gather,
            Resource = resource,
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
            IsTargetGameObject = true,
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

    public static Order UseSkill(string skill)
    {
        return new Order
        {
            Type = OrderType.UseSkill,
            Skill = skill,
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
                    TargetGameObject.OnDestroy_();
                }
                break;
        }
    }

    public bool IsTargetGameObject { get; private set; } // TODO: Get rid of this. Maybe add a new order type.

    public MyGameObject SourceGameObject { get; private set; }

    public MyGameObject TargetGameObject { get; set; }

    public Vector3 TargetPosition { get; set; } // TODO: Hide setter.

    public Quaternion TargetRotation { get; private set; }

    public Timer Timer { get; set; } // TODO: Hide setter.

    public OrderType Type { get; private set; }

    public string Prefab { get; private set; }

    public string Recipe { get; set; }

    public string Resource { get; set; }

    public int Value { get; set; }

    public string Skill { get; private set; }

    public string Technology { get; private set; }
}
