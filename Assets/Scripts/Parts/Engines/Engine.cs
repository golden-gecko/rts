using UnityEngine;

[DisallowMultipleComponent]
public class Engine : Part
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
    }

    public override string GetInfo()
    {
        return string.Format("Engine - {0}, Power: {1:0.}, Fuel: {2}/{3} Speed: {4:0.}", base.GetInfo(), Power.Total, Fuel.GetInfo(), FuelUsage, Speed);
    }

    public bool CanDrive(float distance)
    {
        return Alive && (DistanceToDrive > distance || Fuel.CanDec());
    }

    public void Drive(float distance)
    {
        if (CanDrive(distance) == false)
        {
            return;
        }

        if (distance > DistanceToDrive)
        {
            Fuel.Dec();

            DistanceToDrive += FuelUsage;
        }

        DistanceToDrive -= distance;
    }

    [field: SerializeField]
    public Property Power { get; private set; } = new Property(100.0f);

    [field: SerializeField]
    public Counter Fuel { get; private set; } = new Counter(100, 100);

    [field: SerializeField]
    public float FuelUsage { get; private set; } = 1.0f; // Number of distance units driven per one fuel unit.

    public float Speed { get => Parent.Mass > 0 ? Power.Total / (float)Parent.Mass : 0.0f; }

    private float DistanceToDrive = 0.0f;
}
