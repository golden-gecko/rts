using UnityEngine;

public enum OrderType
{
    Guard,
    Idle,
    Move,
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
        Timer = 0;
        TimerMax = 3;
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

    public OrderType Type { get; }

    public MyGameObject TargetGameObject { get; }

    public Vector3 TargetPosition { get; }

    public float Timer { get; set; }

    public float TimerMax { get; }
}
