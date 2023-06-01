using UnityEngine;

public enum OrderType
{
    Guard,
    Idle,
    Move,
    None,
    Patrol,
    Produce,
    Stop,
}

public class Order
{
    public Order(OrderType type)
    {
        Type = type;
        TargetGameObject = null;
        TargetPosition = Vector3.zero;
        Timer = new Timer();
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

    public Order(OrderType type, float max)
    {
        Type = type;
        TargetGameObject = null;
        TargetPosition = Vector3.zero;
        Timer = new Timer(max);
    }

    public OrderType Type { get; }

    public MyGameObject TargetGameObject { get; }

    public Vector3 TargetPosition { get; }

    public Timer Timer { get; }
}
