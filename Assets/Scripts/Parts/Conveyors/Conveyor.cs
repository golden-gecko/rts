using UnityEngine;

[DisallowMultipleComponent]
public class Conveyor : Part
{
    protected override void Awake()
    {
        base.Awake();

        /*
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
        */

        if (Parent.Direction.x > 0.0)
        {
            ConveyorDiretion = ConveyorDiretion.Down;
        }
        else if (Parent.Direction.x < 0.0)
        {
            ConveyorDiretion = ConveyorDiretion.Up;
        }
        else if (Parent.Direction.z > 0.0)
        {
            ConveyorDiretion = ConveyorDiretion.Right;
        }
        else if (Parent.Direction.z < 0.0)
        {
            ConveyorDiretion = ConveyorDiretion.Left;
        }
    }

    protected override void Update()
    {
        base.Update();

        foreach (Resource resource in Parent.Storage.Resources.Items)
        {
        }
    }

    private MyGameObject GetInput()
    {
        return null;
    }

    private MyGameObject GetOutput()
    {
        return null;
    }

    public ConveyorDiretion ConveyorDiretion { get; private set; } = ConveyorDiretion.Right;
}
