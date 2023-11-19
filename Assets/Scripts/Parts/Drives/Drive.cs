using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Drive : Part
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.Explore);
        Parent.Orders.AllowOrder(OrderType.Follow);
        Parent.Orders.AllowOrder(OrderType.Move);
        Parent.Orders.AllowOrder(OrderType.Patrol);
        Parent.Orders.AllowOrder(OrderType.Teleport);
        Parent.Orders.AllowOrder(OrderType.Turn);

        Parent.OrderHandlers[OrderType.Explore] = new OrderHandlerExplore();
        Parent.OrderHandlers[OrderType.Follow] = new OrderHandlerFollow();
        Parent.OrderHandlers[OrderType.Move] = new OrderHandlerMove();
        Parent.OrderHandlers[OrderType.Patrol] = new OrderHandlerPatrol();
        Parent.OrderHandlers[OrderType.Teleport] = new OrderHandlerTeleport();
        Parent.OrderHandlers[OrderType.Turn] = new OrderHandlerTurn();

        foreach (MyGameObjectMapLayer layer in MapLayers)
        {
            if (Parent.MapLayers.Contains(layer) == false)
            {
                Parent.MapLayers.Add(layer);
            }
        }
    }

    [field: SerializeField]
    public List<MyGameObjectMapLayer> MapLayers { get; private set; } = new List<MyGameObjectMapLayer>();
}
