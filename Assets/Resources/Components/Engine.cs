using UnityEngine;

public class Engine : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.Explore);
        Parent.Orders.AllowOrder(OrderType.Follow);
        Parent.Orders.AllowOrder(OrderType.Move);
        Parent.Orders.AllowOrder(OrderType.Patrol);
        Parent.Orders.AllowOrder(OrderType.Turn);

        Parent.OrderHandlers[OrderType.Explore] = new OrderHandlerExplore();
        Parent.OrderHandlers[OrderType.Follow] = new OrderHandlerFollow();
        Parent.OrderHandlers[OrderType.Move] = new OrderHandlerMove();
        Parent.OrderHandlers[OrderType.Patrol] = new OrderHandlerPatrol();
        Parent.OrderHandlers[OrderType.Turn] = new OrderHandlerTurn();
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Power: {1:0.}, Fuel: {2}/{3} Speed: {4:0.}", base.GetInfo(), Power.Total, Fuel.GetInfo(), FuelUsage, Speed);
    }

    public bool CanDrive(float distance)
    {
        return distanceToDrive > distance || Fuel.CanDec();
    }

    public void Drive(float distance)
    {
        if (CanDrive(distance) == false)
        {
            return;
        }

        if (distance > distanceToDrive)
        {
            Fuel.Dec();

            distanceToDrive += FuelUsage;
        }

        distanceToDrive -= distance;
    }

    [field: SerializeField]
    public Property Power { get; private set; } = new Property(100.0f);

    [field: SerializeField]
    public Counter Fuel { get; } = new Counter(100, 100);

    [field: SerializeField]
    public float FuelUsage { get; private set; } = 1.0f;

    public float Speed { get => Power.Total / Parent.Mass; }

    private float distanceToDrive = 0.0f;
}
