using UnityEngine;

public class Engine : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Explore);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Follow);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Guard);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Move);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Patrol);

        GetComponent<MyGameObject>().OrderHandlers[OrderType.Explore] = new OrderHandlerExplore();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Follow] = new OrderHandlerFollow();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Guard] = new OrderHandlerGuard();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Move] = new OrderHandlerMove();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Patrol] = new OrderHandlerPatrol();
    }

    public bool CanDrive(float distance)
    {
        return Fuel >= distance * FuelUsage;
    }

    public void Drive(float distance)
    {
        if (CanDrive(distance) == false)
        {
            return;
        }

        Fuel = Mathf.Clamp(Fuel - distance * FuelUsage, 0.0f, FuelMax);
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Power: {1:0.}, Fuel: {2:0.}/{3:0.}/{4:0.} Speed: {5:0.}", base.GetInfo(), Power, Fuel, FuelMax, FuelUsage, Speed);
    }

    [field: SerializeField]
    public float Power { get; set; } = 50.0f;

    [field: SerializeField]
    public float Fuel { get; set; } = 100.0f;

    [field: SerializeField]
    public float FuelMax { get; set; } = 100.0f;

    [field: SerializeField]
    public float FuelUsage { get; set; } = 1.0f;

    public float Speed { get => Power / GetComponent<MyGameObject>().Mass; }
}
